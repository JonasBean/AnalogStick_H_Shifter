using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static AnalogStick_H_Shifter.ControllerHandling;

namespace AnalogStick_H_Shifter
{
    public partial class H_Shifter : Form
    {
        XInputController xinput;
        DirectInputController dinput;

        public bool useXInput = true;

        public string root = Path.GetDirectoryName(Application.ExecutablePath);
        public string layoutFolder;
        public string imagesFolder;

        public Point rightThumb = new Point(0, 0);
        public Point mouseCoords = new Point(0, 0);

        public Bitmap axisImage;
        public Bitmap overlayImage;

        static readonly int axisSize = 680;
        static readonly int imageSize = axisSize + 20;
        static readonly int gearsCount = 7;
        int previousGear = 0;
        int previousGearInBackground = -1;

        int currentlyGrabbedRectangle = -99;
        PointF relativeGrabPosition;

        bool paintItOnce = true;
        bool update = false;
        bool refreshOverlay = false;
        bool grabber;

        private BackgroundWorker bGWorker = null;
        private BackgroundWorker paintingbGWorker = null;

        readonly Brush[] rectColors = new Brush[] { 
            new SolidBrush(Color.FromArgb(255, 46, 204, 113)), // Green (Gear 1)
            new SolidBrush(Color.FromArgb(255, 230, 126, 34)), // Orange (Gear 2)
            new SolidBrush(Color.FromArgb(255, 149, 165, 166)), // Gray (Gear 3)
            new SolidBrush(Color.FromArgb(255, 52, 152, 219)), // Light Blue (Gear 4)
            new SolidBrush(Color.FromArgb(255, 155, 89, 182)), // Purple (Gear 5)
            new SolidBrush(Color.FromArgb(255, 241, 196, 15)), // Yellow (Gear 6)
            new SolidBrush(Color.FromArgb(255, 231, 76, 60))   // Red (Reverse gear)
        };
        readonly string[] rectangleStrings = new string[] { "1", "2", "3", "4", "5", "6", "R", };

        List<GearRectangle> gearRectangles = new List<GearRectangle>();
        readonly ControllerHandling controllerHandling = new ControllerHandling();

        int[] shiftCounter = new int[7];

        public H_Shifter()
        {
            for (int i = 0; i < gearsCount; i++)
            {
                GearRectangle gear = new GearRectangle();
                gear.Gear = rectangleStrings[i];

                gearRectangles.Add(gear);
            }

            InitializeComponent();

            overlayBox.MouseDown += overlayBox_MouseDown;
            overlayBox.MouseUp += overlayBox_MouseUp;

            layoutFolder = Path.Combine(root, "Layouts");
            imagesFolder = Path.Combine(root, "Images");

            axisImage = new Bitmap(imageSize, imageSize);
            overlayImage = new Bitmap(imageSize, imageSize);

            overlayBox.BackColor = Color.Transparent;
            overlayBox.Parent = axisBox;
            overlayBox.Location = new Point(0, 0);

            PopulateListBox();

            if (savedLayoutsListBox.Items.Count != 0)
            {
                savedLayoutsListBox.SelectedIndex = 0;
                gearRectangles = ReadGearListFromFile(layoutFolder + "\\" + savedLayoutsListBox.SelectedItem.ToString());
            }
            else
            {
                // Standard H-shift layout: 1-3-5 top, 2-4-6 bottom
                gearRectangles = new List<GearRectangle> {
                    new GearRectangle(100, 100, 30, 30, rectangleStrings[0], new Rectangle(180, 200, 120, 120)),  // Gear 1: Top Left
                    new GearRectangle(200, 100, 300, 30, rectangleStrings[1], new Rectangle(180, 500, 120, 120)), // Gear 2: Bottom Left  
                    new GearRectangle(120, 120, 600, 30, rectangleStrings[2], new Rectangle(330, 200, 120, 120)), // Gear 3: Top Center
                    new GearRectangle(120, 120, 30, 300, rectangleStrings[3], new Rectangle(330, 500, 120, 120)), // Gear 4: Bottom Center
                    new GearRectangle(120, 120, 300, 300, rectangleStrings[4], new Rectangle(480, 200, 120, 120)), // Gear 5: Top Right
                    new GearRectangle(120, 120, 600,300, rectangleStrings[5], new Rectangle(480, 500, 120, 120)),  // Gear 6: Bottom Right
                    new GearRectangle(120, 120, 300, 600, rectangleStrings[6], new Rectangle(20, 350, 120, 120))   // Reverse: Left Center
                };
            }

            PaintGears();

            gearRightNow.Font = new Font("Microsoft Sans Serif", 55, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

            try
            {
                shiftCounter = ReadFromBinaryFile<int[]>(Path.GetDirectoryName(Application.ExecutablePath) + "//shiftCount");

                frstGearLabel.Text = shiftCounter[0].ToString();
                scndGearLabel.Text = shiftCounter[1].ToString();
                thrdGearLabel.Text = shiftCounter[2].ToString();
                frthGearLabel.Text = shiftCounter[3].ToString();
                ffthGearLabel.Text = shiftCounter[4].ToString();
                sxthGearLabel.Text = shiftCounter[5].ToString();
                rvrsGearLabel.Text = shiftCounter[6].ToString();

                ttlshift.Text = getTotalShifts().ToString();

                Console.WriteLine("Shiftcount-File loaded successfully");
            }
            catch (Exception)
            {
                Console.WriteLine("Shiftcount-File not found");
            }

            refreshOverlay = true;
            overlayBox_Click(null, null);
            overlayBox.Image = overlayImage;

            xinput = new XInputController();
            dinput = new DirectInputController(joyStickListBox, false);

            xinput.Update();
            dinput.Update();
        }

        private void PopulateListBox()
        {
            savedLayoutsListBox.Items.Clear();
            DirectoryInfo dinfo = new DirectoryInfo(path: layoutFolder);

            if (!dinfo.Exists)
            {
                dinfo.Create();
            }

            FileInfo[] Files = dinfo.GetFiles("*.*");

            foreach (FileInfo file in Files)
            {
                savedLayoutsListBox.Items.Add(file.Name);
            }

            if (savedLayoutsListBox.Items.Count > 0)
            {
                savedLayoutsListBox.SelectedIndex = 0;
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            refreshOverlay = true;
            overlayBox_Click(null, null);
        }

        private void overlayBox_MouseDown(object sender, EventArgs e)
        {
            overlayBox.MouseMove += overlayBox_MouseMoved;

            MouseEventArgs me = (MouseEventArgs)e;
            mouseCoords = new Point(me.Location.X, me.Location.Y);

            CheckForGrabHandle(mouseCoords);
        }

        private void overlayBox_MouseUp(object sender, EventArgs e)
        {
            overlayBox.MouseMove -= overlayBox_MouseMoved;

            //if (overlayImage != null)
            //    overlayImage.Dispose();

            currentlyGrabbedRectangle = -99;

            if (paintingbGWorker != null)
            {
                System.Threading.Thread.Sleep(60);
                paintingbGWorker.CancelAsync();
            }
        }

        private void overlayBox_MouseMoved(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (currentlyGrabbedRectangle != -99)
            {
                if (grabber)
                {
                    if (me.Location.X >= 0 && me.Location.X <= 535)
                    {
                        gearRectangles[currentlyGrabbedRectangle].XPosition = me.Location.X + (int)relativeGrabPosition.X;
                    }

                    if (me.Location.Y >= 0 && me.Location.Y <= 535)
                    {
                        gearRectangles[currentlyGrabbedRectangle].YPosition = me.Location.Y + (int)relativeGrabPosition.Y;
                    }
                }
                else
                {
                    if (me.Location.X >= 0 && me.Location.X <= 680 &&
                        me.Location.X - gearRectangles[currentlyGrabbedRectangle].XPosition > 100)
                    {
                        gearRectangles[currentlyGrabbedRectangle].Width = me.Location.X - gearRectangles[currentlyGrabbedRectangle].XPosition + (int)relativeGrabPosition.X;
                    }

                    if (me.Location.Y >= 0 && me.Location.Y <= 690 &&
                        me.Location.Y - gearRectangles[currentlyGrabbedRectangle].YPosition > 100)
                    {
                        gearRectangles[currentlyGrabbedRectangle].Height = me.Location.Y - gearRectangles[currentlyGrabbedRectangle].YPosition + (int)relativeGrabPosition.Y;
                    }
                }
            }
        }

        private void CheckForGrabHandle(Point mousePoint)
        {
            for (int i = 0; i < gearRectangles.Count; i++)
            {
                if (PointIsInside(gearRectangles[i].Rect, mousePoint, true))
                {
                    grabber = true;
                    currentlyGrabbedRectangle = i;

                    relativeGrabPosition = new PointF(
                        gearRectangles[currentlyGrabbedRectangle].XPosition - mouseCoords.X,
                        gearRectangles[currentlyGrabbedRectangle].YPosition - mouseCoords.Y
                    );

                    if (paintingbGWorker == null || !paintingbGWorker.IsBusy)
                    {
                        paintingbGWorker = new BackgroundWorker();
                        paintingbGWorker.DoWork += new DoWorkEventHandler(paintingbGWorker_DoWork);
                        paintingbGWorker.WorkerReportsProgress = true;
                        paintingbGWorker.WorkerSupportsCancellation = true;
                        paintingbGWorker.RunWorkerAsync();
                    }
                }

                if (PointIsInside(gearRectangles[i].Rect, mousePoint, false))
                {
                    grabber = false;
                    currentlyGrabbedRectangle = i;

                    relativeGrabPosition = new PointF(
                        gearRectangles[currentlyGrabbedRectangle].XPosition + gearRectangles[currentlyGrabbedRectangle].Width - mouseCoords.X,
                        gearRectangles[currentlyGrabbedRectangle].YPosition + gearRectangles[currentlyGrabbedRectangle].Height - mouseCoords.Y
                    );

                    if (paintingbGWorker == null || !paintingbGWorker.IsBusy)
                    {
                        paintingbGWorker = new BackgroundWorker();
                        paintingbGWorker.DoWork += new DoWorkEventHandler(paintingbGWorker_DoWork);
                        paintingbGWorker.WorkerReportsProgress = true;
                        paintingbGWorker.WorkerSupportsCancellation = true;
                        paintingbGWorker.RunWorkerAsync();
                    }
                }
            }
        }

        private void overlayBox_Click(object sender, EventArgs e)
        {
            if (!refreshOverlay)
            {
                MouseEventArgs me = (MouseEventArgs)e;
                mouseCoords = new Point(me.Location.X, me.Location.Y);

                switch (previousGear)
                {
                    case 1:
                        gearRectangles[0].XPosition = me.Location.X - (gearRectangles[0].Width / 2);
                        gearRectangles[0].YPosition = me.Location.Y - (gearRectangles[0].Height / 2);
                        break;
                    case 2:
                        gearRectangles[1].XPosition = me.Location.X - (gearRectangles[1].Width / 2);
                        gearRectangles[1].YPosition = me.Location.Y - (gearRectangles[1].Height / 2);
                        break;
                    case 3:
                        gearRectangles[2].XPosition = me.Location.X - (gearRectangles[2].Width / 2);
                        gearRectangles[2].YPosition = me.Location.Y - (gearRectangles[2].Height / 2);
                        break;
                    case 4:
                        gearRectangles[3].XPosition = me.Location.X - (gearRectangles[3].Width / 2);
                        gearRectangles[3].YPosition = me.Location.Y - (gearRectangles[3].Height / 2);
                        break;
                    case 5:
                        gearRectangles[4].XPosition = me.Location.X - (gearRectangles[4].Width / 2);
                        gearRectangles[4].YPosition = me.Location.Y - (gearRectangles[4].Height / 2);
                        break;
                    case 6:
                        gearRectangles[5].XPosition = me.Location.X - (gearRectangles[5].Width / 2);
                        gearRectangles[5].YPosition = me.Location.Y - (gearRectangles[5].Height / 2);
                        break;
                    case 9:
                        gearRectangles[6].XPosition = me.Location.X - (gearRectangles[6].Width / 2);
                        gearRectangles[6].YPosition = me.Location.Y - (gearRectangles[6].Height / 2);
                        break;
                    default: break;
                }
            }
            else
            {
                refreshOverlay = false;
            }
        }

        private void resetImageButton_Click(object sender, EventArgs e)
        {
            // Set the flag for the next paint operation
            paintItOnce = true;
            
            // Immediately perform a reset of the image, even when not active
            ResetAxisImage();
        }

        private void ResetAxisImage()
        {
            try
            {
                if (axisImage != null)
                {
                    using (Graphics graph = Graphics.FromImage(axisImage))
                    {
                        // Redraw the background (Reset)
                        graph.FillRectangle(new SolidBrush(Color.FromArgb(55, 55, 58)), new Rectangle(0, 0, imageSize, imageSize));
                    }
                    
                    // Update the display immediately
                    axisBox.Image = axisImage;
                    axisBox.Invalidate();
                    axisBox.Refresh();
                }
            }
            catch (Exception ex)
            {
                // Error handling in case the image is not available
                Console.WriteLine("Error resetting axis image: " + ex.Message);
            }
        }

        private void activateControllerButton_Click(object sender, EventArgs e)
        {
            update = !update;

            if (update)
            {
                if (bGWorker == null)
                {
                    bGWorker = new BackgroundWorker();
                    bGWorker.DoWork += new DoWorkEventHandler(bGWorker_DoWork);
                    bGWorker.ProgressChanged += new ProgressChangedEventHandler(bGWorker_ProgressChanged);
                }

                bGWorker.WorkerReportsProgress = true;
                bGWorker.WorkerSupportsCancellation = true;
                bGWorker.RunWorkerAsync();

                // Green background for active state
                activateControllerButton.BackColor = Color.FromArgb(255, 46, 204, 113); // Green
                activateControllerButton.ForeColor = Color.Black; // Black text for better contrast
                activateControllerButton.Text = "Active";
            }
            else
            {
                bGWorker.CancelAsync();
                // Back to original color for inactive state
                activateControllerButton.BackColor = Color.FromArgb(255, 45, 45, 48); // Dark gray
                activateControllerButton.ForeColor = Color.White; // White text on dark background
                activateControllerButton.Text = "Activate";
            }
        }

        void bGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            do
            {
                if (bGWorker.CancellationPending)
                    break;

                double relStickX;
                double relStickY;

                if (useXInput)
                {
                    xinput.Update();
                    relStickX = axisSize * ((double)xinput.gamepad.RightThumbX + 32768) / 65536;
                    relStickY = axisSize * ((double)xinput.gamepad.RightThumbY + 32768) / 65536;
                }
                else
                {
                    dinput.Update();
                    relStickX = axisSize * ((double)dinput.joystickPosition.X) / 65536;
                    relStickY = axisSize * ((double)dinput.joystickPosition.Y) / 65536;
                }

                int relStickXInt = Convert.ToInt32(relStickX);
                int relStickYInt = Convert.ToInt32(relStickY);

                Point stick;
                Rectangle axisPosition;

                if (useXInput)
                {
                    stick = new Point(relStickXInt, axisSize - relStickYInt);
                    axisPosition = new Rectangle(relStickXInt, axisSize - relStickYInt, 10, 10);
                }
                else
                {
                    stick = new Point(relStickXInt, relStickYInt);
                    axisPosition = new Rectangle(relStickXInt, relStickYInt, 10, 10);
                }

                try
                {
                    using (Graphics graph = Graphics.FromImage(axisImage))
                    {
                        if (paintItOnce)
                        {
                            graph.FillRectangle(new SolidBrush(Color.FromArgb(55, 55, 58)), new Rectangle(0, 0, imageSize, imageSize));
                            paintItOnce = false;
                        }

                        // first gear ScanCodeShort.KEY_1
                        if (gearRectangles[0].Rect.Contains(stick))
                        {
                            graph.FillEllipse(rectColors[0], axisPosition);
                            bGWorker.ReportProgress(2);
                        }
                        // second gear ScanCodeShort.KEY_2
                        else if (gearRectangles[1].Rect.Contains(stick))
                        {
                            graph.FillEllipse(rectColors[1], axisPosition);
                            bGWorker.ReportProgress(3);
                        }
                        // third gear ScanCodeShort.KEY_3
                        else if (gearRectangles[2].Rect.Contains(stick))
                        {
                            graph.FillEllipse(rectColors[2], axisPosition);
                            bGWorker.ReportProgress(4);
                        }
                        // fourth gear ScanCodeShort.KEY_4
                        else if (gearRectangles[3].Rect.Contains(stick))
                        {
                            graph.FillEllipse(rectColors[3], axisPosition);
                            bGWorker.ReportProgress(5);
                        }
                        // fifth gear ScanCodeShort.KEY_5
                        else if (gearRectangles[4].Rect.Contains(stick))
                        {
                            graph.FillEllipse(rectColors[4], axisPosition);
                            bGWorker.ReportProgress(6);
                        }
                        // sixth gear ScanCodeShort.KEY_6
                        else if (gearRectangles[5].Rect.Contains(stick))
                        {
                            graph.FillEllipse(rectColors[5], axisPosition);
                            bGWorker.ReportProgress(7);
                        }
                        // reverse gear ScanCodeShort.KEY_9
                        else if (gearRectangles[6].Rect.Contains(stick))
                        {
                            graph.FillEllipse(rectColors[6], axisPosition);
                            bGWorker.ReportProgress(10);
                        }
                        else // ScanCodeShort.KEY_9
                        {
                            gearRightNow.ForeColor = Color.Black;
                            graph.FillEllipse(Brushes.Black, axisPosition);
                            bGWorker.ReportProgress(11);
                        }
                    }

                    axisBox.Image = axisImage;
                    System.Threading.Thread.Sleep(30);
                }
                catch (Exception) { }
            } while (true);
        }

        void bGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (previousGearInBackground != e.ProgressPercentage)
            {
                if (e.ProgressPercentage == 11)
                {
                    controllerHandling.Release((ScanCodeShort)previousGearInBackground);
                }
                else
                {
                    //Send Release Send for a cleaner signal when shifting directly from gear to gear
                    controllerHandling.SendInputWithAPI((ScanCodeShort)e.ProgressPercentage);
                    controllerHandling.Release((ScanCodeShort)e.ProgressPercentage);
                    controllerHandling.SendInputWithAPI((ScanCodeShort)e.ProgressPercentage);
                }

                previousGearInBackground = e.ProgressPercentage;

                switch (previousGearInBackground - 1)
                {
                    case 1: // Gear 1
                        gearRightNow.ForeColor = Color.FromArgb(255, 46, 204, 113); // Green (like rectColors[0])
                        gearRightNow.Text = (previousGearInBackground - 1).ToString();
                        shiftCounter[0]++;
                        frstGearLabel.Text = shiftCounter[0].ToString();
                        break;
                    case 2: // Gear 2
                        gearRightNow.ForeColor = Color.FromArgb(255, 230, 126, 34); // Orange (like rectColors[1])
                        gearRightNow.Text = (previousGearInBackground - 1).ToString();
                        shiftCounter[1]++;
                        scndGearLabel.Text = shiftCounter[1].ToString();
                        break;
                    case 3: // Gear 3
                        gearRightNow.ForeColor = Color.FromArgb(255, 149, 165, 166); // Gray (like rectColors[2])
                        gearRightNow.Text = (previousGearInBackground - 1).ToString();
                        shiftCounter[2]++;
                        thrdGearLabel.Text = shiftCounter[2].ToString();
                        break;
                    case 4: // Gear 4
                        gearRightNow.ForeColor = Color.FromArgb(255, 52, 152, 219); // Light blue (like rectColors[3])
                        gearRightNow.Text = (previousGearInBackground - 1).ToString();
                        shiftCounter[3]++;
                        frthGearLabel.Text = shiftCounter[3].ToString();
                        break;
                    case 5: // Gear 5
                        gearRightNow.ForeColor = Color.FromArgb(255, 155, 89, 182); // Purple (like rectColors[4])
                        gearRightNow.Text = (previousGearInBackground - 1).ToString();
                        shiftCounter[4]++;
                        ffthGearLabel.Text = shiftCounter[4].ToString();
                        break;
                    case 6: // Gear 6
                        gearRightNow.ForeColor = Color.FromArgb(255, 241, 196, 15); // Yellow (like rectColors[5])
                        gearRightNow.Text = (previousGearInBackground - 1).ToString();
                        shiftCounter[5]++;
                        sxthGearLabel.Text = shiftCounter[5].ToString();
                        break;
                    case 9: // Reverse gear
                        gearRightNow.ForeColor = Color.FromArgb(255, 231, 76, 60); // Red (like rectColors[6])
                        gearRightNow.Text = "R";
                        shiftCounter[6]++;
                        rvrsGearLabel.Text = shiftCounter[6].ToString();
                        break;
                    default:
                        gearRightNow.ForeColor = Color.Black;
                        gearRightNow.Text = "N";
                        break;
                }

                ttlshift.Text = getTotalShifts().ToString();
                WriteToBinaryFile(Path.GetDirectoryName(Application.ExecutablePath) + "//shiftCount", shiftCounter);

            }
        }

        void paintingbGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            do
            {
                if (paintingbGWorker.CancellationPending)
                    break;

                try
                {
                    PaintGears();

                    System.Threading.Thread.Sleep(20);
                }
                catch (Exception) { }
            } while (true);
        }

        void grabbingbGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            do
            {
                if (paintingbGWorker.CancellationPending)
                    break;

                try
                {
                    PaintGears();

                    System.Threading.Thread.Sleep(20);
                }
                catch (Exception) { }
            } while (true);
        }

        private void PaintGears()
        {
            overlayImage = new Bitmap(imageSize, imageSize);

            using (Graphics graph = Graphics.FromImage(overlayImage))
            {
                for (int i = 0; i < gearRectangles.Count; i++)
                {
                    // Gear-Rectangle
                    using(var path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        path.AddRectangle(gearRectangles[i].Rect);
                        graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        graph.DrawPath(new Pen(rectColors[i], 3), path);
                    }

                    // Gear-Name with modern styling
                    var gearNameRect = new Rectangle(
                        gearRectangles[i].XPosition + gearRectangles[i].Width - 22,
                        gearRectangles[i].YPosition - 12,
                        30, 30);
                    
                    using(var path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        path.AddRectangle(gearNameRect);
                        graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        
                        // Fill with semi-transparent white
                        graph.FillPath(new SolidBrush(Color.FromArgb(240, 255, 255, 255)), path);
                        
                        // Draw border
                        graph.DrawPath(new Pen(rectColors[i], 2), path);
                    }
                    
                    // Draw gear number with modern font
                    using(Font drawFont = new Font("Segoe UI", 16, FontStyle.Bold))
                    {
                        var format = new StringFormat()
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };
                        graph.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                        graph.DrawString(gearRectangles[i].Gear, drawFont, rectColors[i], gearNameRect, format);
                    }

                    // Gear-Grabber
                    graph.FillRectangle(new SolidBrush(Color.White), gearRectangles[i].XPosition - 8, gearRectangles[i].YPosition - 10, 20, 23);
                    graph.DrawImage(Properties.Resources.directionArrows, new Point(gearRectangles[i].XPosition - 8, gearRectangles[i].YPosition - 8));

                    // Gear-Scaler
                    graph.FillRectangle(new SolidBrush(Color.White), gearRectangles[i].XPosition + gearRectangles[i].Width - 12, gearRectangles[i].YPosition + gearRectangles[i].Height - 12, 20, 20);
                    graph.DrawImage(Properties.Resources.scalingArrow, gearRectangles[i].XPosition + gearRectangles[i].Width - 10, gearRectangles[i].YPosition + gearRectangles[i].Height - 10, 18, 18);
                }
            }

            overlayBox.Image = overlayImage;
        }

        int getTotalShifts()
        {
            int tmp = 0;

            for (int i = 0; i < shiftCounter.Length; i++)
            {
                tmp += shiftCounter[i];
            }

            return tmp;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            aboutForm win2 = new aboutForm();
            win2.Show();
        }

        public static void WriteGearsToFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            List<GearRectangle> dummyForType = new List<GearRectangle>();

            if (objectToWrite.GetType() == dummyForType.GetType())
            {
                List<GearRectangle> gearRectanglesToSave = objectToWrite as List<GearRectangle>;

                using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
                {
                    foreach (var gearRect in gearRectanglesToSave)
                    {
                        writer.Write(gearRect.Gear);

                        writer.Write(gearRect.XPosition);
                        writer.Write(gearRect.YPosition);
                        writer.Write(gearRect.Width);
                        writer.Write(gearRect.Height);
                    }
                }
            }
        }

        List<GearRectangle> ReadGearListFromFile(string filePath)
        {
            FileStream fin = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader reader = new BinaryReader(fin);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);

            List<GearRectangle> loadRects = new List<GearRectangle>();

            for (int i = 0; i < gearsCount; i++)
            {
                GearRectangle gearRect = new GearRectangle();

                gearRect.Gear = reader.ReadString();

                gearRect.XPosition = reader.ReadInt32();
                gearRect.YPosition = reader.ReadInt32();
                gearRect.Width = reader.ReadInt32();
                gearRect.Height = reader.ReadInt32();

                loadRects.Add(gearRect);
            }

            reader.Close();

            return loadRects;
        }

        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }

        private void directInputRadiobutton_Clicked(object sender, EventArgs e)
        {
            directInputRadiobutton.Checked = true;
            xInputRadioButton.Checked = false;
            useXInput = false;
        }

        private void xInputRadioButton_Clicked(object sender, EventArgs e)
        {
            xInputRadioButton.Checked = true;
            directInputRadiobutton.Checked = false;
            useXInput = true;
        }

        private void joyStickListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            dinput = new DirectInputController(joyStickListBox, true);
        }

        private void loadButton_Click(object sender, EventArgs e)
        {

            if (savedLayoutsListBox.SelectedItem != null)
            {
                gearRectangles = ReadGearListFromFile(layoutFolder + "\\" + savedLayoutsListBox.SelectedItem.ToString());

                PaintGears();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            WriteGearsToFile(layoutFolder + "//" + layoutNameBox.Text, gearRectangles);
            PopulateListBox();

            for (int i = 0; i < savedLayoutsListBox.Items.Count; i++)
            {
                if (savedLayoutsListBox.Items[i].ToString() == layoutNameBox.Text)
                {
                    //savedLayoutsListBox.SelectedIndex = i;
                }
            }
        }

        private void savedLayoutsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            layoutNameBox.Text = savedLayoutsListBox.SelectedItem.ToString();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/c/BeanJ?sub_confirmation=1");
        }

        bool PointIsInside(Rectangle rect, PointF mouseclick, bool grabber)
        {
            int distanceFromCorner = 10;
            double area = distanceFromCorner * 2 * distanceFromCorner * 2;

            PointF[] v;

            if (grabber)
            {
                v = new PointF[] {
                    new PointF(rect.X - distanceFromCorner, rect.Y - distanceFromCorner),
                    new PointF(rect.X + distanceFromCorner, rect.Y - distanceFromCorner),
                    new PointF(rect.X + distanceFromCorner, rect.Y + distanceFromCorner),
                    new PointF(rect.X - distanceFromCorner, rect.Y + distanceFromCorner)
                };
            }
            else
            {
                v = new PointF[] {
                    new PointF(rect.X + rect.Width - distanceFromCorner, rect.Y + rect.Height - distanceFromCorner),
                    new PointF(rect.X + rect.Width + distanceFromCorner, rect.Y + rect.Height - distanceFromCorner),
                    new PointF(rect.X + rect.Width + distanceFromCorner, rect.Y + rect.Height + distanceFromCorner),
                    new PointF(rect.X + rect.Width - distanceFromCorner, rect.Y + rect.Height + distanceFromCorner)
                };
            }

            // calculate area of all rectangle-vertices to the mouseclick coordinates
            float measuredArea =
                TriangleArea(mouseclick, v[0], v[1]) +
                TriangleArea(mouseclick, v[1], v[2]) +
                TriangleArea(mouseclick, v[2], v[3]) +
                TriangleArea(mouseclick, v[3], v[0]);

            // check for negative values
            if (measuredArea < 0)
            {
                measuredArea *= -1;
            }

            // if the measuredArea is bigger the mouseclick is outside of the found contour
            if (measuredArea > area + 1)
            {
                return false;
            }
            return true;
        }

        float TriangleArea(PointF A, PointF B, PointF C)
        {
            float triangleArea = ((B.X * A.Y - A.X * B.Y) + (C.X * B.Y - B.X * C.Y) + (A.X * C.Y - C.X * A.Y)) / 2;

            // check for negative values
            if (triangleArea < 0)
            {
                triangleArea *= -1;
            }
            // calculate area of triangle with points
            return triangleArea;
        }
    }
}