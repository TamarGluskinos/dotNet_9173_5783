using Gst.Rtsp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VisioForge.Libs.MediaFoundation.OPM;
using Simulator;
using VisioForge.Libs.NDI;
using System.Windows.Media.Animation;

namespace PL;


/// <summary>
/// Interaction logic for Simulator.xaml
/// </summary>
public partial class SimulatorWindow : Window
{
    /// <summary>
    /// background worker
    /// </summary>
    BackgroundWorker worker;

    /// <summary>
    /// stop watch
    /// </summary>
    private Stopwatch stopWatch;

    /// <summary>
    /// disables user from closing the window in the middle of a simulation
    /// </summary>
    private const int GWL_STYLE = -16;
    private const int WS_SYSMENU = 0x80000;
    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    /// <summary>
    /// tuple of simulation details- order and time
    /// </summary>
    Tuple<BO.Order, int> detailsTuple;

    /// <summary>
    /// next status which the simulation is moving the order to
    /// </summary>
    public BO.eOrderStatus nextStatus;

    /// <summary>
    /// parameters for progress bar
    /// </summary>
    Duration duration;
    DoubleAnimation? doubleanimation;
    ProgressBar? ProgressBar;

    /// <summary>
    /// SimulatorWindow constructor
    /// </summary>
    public SimulatorWindow()
    {
        InitializeComponent();
        stopWatch = new Stopwatch();
        detailsTuple = new Tuple<BO.Order, int>(new BO.Order(), 0);
        worker = new BackgroundWorker();
        worker.DoWork += Worker_DoWork;
        worker.ProgressChanged += Worker_ProgressChanged;
        worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        worker.WorkerReportsProgress = true;
        worker.WorkerSupportsCancellation = true;
        worker.RunWorkerAsync();
        DataContext = detailsTuple;
        stopWatch.Restart();
        Simulator.Simulator.StopSimulator += StopSimulator;
        Simulator.Simulator.UpdateProgress += UpdateProgress;

    }

    /// <summary>
    /// updates background-worker's progress
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event arguments</param>
    public void UpdateProgress(object sender, EventArgs e)
    {

        if (!(e is SimulatorEventDetails))
            return;
        SimulatorEventDetails details = e as SimulatorEventDetails;
        detailsTuple = new Tuple<BO.Order, int>(details.Order, details.time);
        if (!CheckAccess())
        {
            Dispatcher.BeginInvoke(UpdateProgress, sender, e);
        }
        else
        {
            nextStatus = (BO.eOrderStatus)((int)details.Order.Status + 1);
            txtNextState.Text = nextStatus.ToString();
            DataContext = detailsTuple;
            ProgressBarStart(details.time);
        }
    }

    /// <summary>
    /// stops simulation
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event arguments</param>
    public void StopSimulator(object sender, EventArgs e)
    {
        StopSimulator_Click(sender, e as RoutedEventArgs);
    }

    /// <summary>
    /// starts progress bar
    /// </summary>
    /// <param name="sec">how many seconds the progress bar should run for</param>
    void ProgressBarStart(int sec)
    {
        if (ProgressBar != null)
        {
            SBar.Items.Remove(ProgressBar);
        }
        ProgressBar = new ProgressBar();
        ProgressBar.IsIndeterminate = false;
        ProgressBar.Orientation = Orientation.Horizontal;
        ProgressBar.Width = 500;
        ProgressBar.Height = 30;
        duration = new Duration(TimeSpan.FromSeconds(sec * 2));
        doubleanimation = new DoubleAnimation(200.0, duration);
        ProgressBar.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);
        SBar.Items.Add(ProgressBar);
    }

    /// <summary>
    /// background-worker's work
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event arguments</param>
    private void Worker_DoWork(object sender, DoWorkEventArgs e)
    {
        Simulator.Simulator.run();
        while (worker.WorkerSupportsCancellation)
        {
            worker.ReportProgress(1);
            Thread.Sleep(1000);
        }
    }

    /// <summary>
    /// background-worker's progress changed
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event arguments</param>
    private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        string timerText = stopWatch.Elapsed.ToString();
        timerText = timerText.Substring(0, 8);
        timerTextBlock.Text = timerText;
    }

    /// <summary>
    /// back-ground worker's running completed
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event arguments</param>
    private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {

        Simulator.Simulator.StopSimulator -= StopSimulator;
        Simulator.Simulator.UpdateProgress -= UpdateProgress;
        this.Close();
        MessageBox.Show("simulation stoped");
    }

    /// <summary>
    /// stops simulation
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event arguments</param>
    private void StopSimulator_Click(object sender, RoutedEventArgs e)
    {
        Simulator.Simulator.ToStop();
        if (worker.WorkerSupportsCancellation == true)
            worker.WorkerSupportsCancellation = false;
    }

    /// <summary>
    /// loader (part of preventing window from being closeable)
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event arguments</param>
    private void onLoad(object sender, RoutedEventArgs e)
    {
        var hwnd = new WindowInteropHelper(this).Handle;
        SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

    }
}

