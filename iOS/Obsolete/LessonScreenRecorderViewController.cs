using System;
using System.Collections.Generic;
using UIKit;
using AVFoundation;
using Foundation;
using System.IO;
using System.Diagnostics;
using AudioToolbox;

namespace CorpTraining.iOS
{
    public class LessonScreenRecorderViewController : LessonScreenBaseViewController
    {
        public AVAudioRecorder Recorder { get; set; }

        public NSDictionary Settings { get; set; }

        public NSUrl AudioFilePath { get; set; }

        public NSObject Observer;

        public Stopwatch Stopwatch { get; set; }

        AVPlayer Player { get; set; }

        public LessonScreenRecorderViewController(IList<Screen> screens, int index)
            : base(screens, index)
        {
            Stopwatch = null;
            AudioFilePath = null;

            AudioSession.Initialize(); // Required to activate recording
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            var image = new UIImageView();
            View.AddSubview(image);
            image.Image = UIImage.FromBundle("microphone.png");

            var recordingStatusLabel = new UILabel
            {
                Text = "",
                TextColor = Constants.MainColor,
            };
            View.AddSubview(recordingStatusLabel);

            var lengthOfRecordingLabel = new UILabel
            {
                Text = "",
                TextColor = Constants.MainColor,
            };
            View.AddSubview(lengthOfRecordingLabel);

            var startRecordingButton = new UIButton
            {
            };
            View.AddSubview(startRecordingButton);
            startRecordingButton.SetTitle("Start", UIControlState.Normal);
            startRecordingButton.SetTitleColor(Constants.MainColor, UIControlState.Normal);

            var stopRecordingButton = new UIButton
            {
            };
            View.AddSubview(stopRecordingButton);
            stopRecordingButton.SetTitle("Stop", UIControlState.Normal);
            stopRecordingButton.SetTitleColor(Constants.MainColor, UIControlState.Normal);

            var playRecordedSoundButton = new UIButton
            {
            };
            View.AddSubview(playRecordedSoundButton);
            playRecordedSoundButton.SetTitle("Play", UIControlState.Normal);
            playRecordedSoundButton.SetTitleColor(Constants.MainColor, UIControlState.Normal);

            // start recording wireup
            startRecordingButton.TouchUpInside += (sender, e) =>
            {
                AudioSession.Category = AudioSessionCategory.RecordAudio;
                AudioSession.SetActive(true);

                if (!PrepareAudioRecording())
                {
                    recordingStatusLabel.Text = "Error preparing";
                    return;
                }

                if (!Recorder.Record())
                {
                    recordingStatusLabel.Text = "Error recording";
                    return;
                }

                Stopwatch = new Stopwatch();
                Stopwatch.Start();
                lengthOfRecordingLabel.Text = "";
                recordingStatusLabel.Text = "Recording";
                startRecordingButton.Enabled = false;
                stopRecordingButton.Enabled = true;
            };

            // stop recording wireup
            stopRecordingButton.TouchUpInside += (sender, e) =>
            {
                Recorder.Stop();

                lengthOfRecordingLabel.Text = string.Format("{0:hh\\:mm\\:ss}", Stopwatch.Elapsed);
                Stopwatch.Stop();
                recordingStatusLabel.Text = "";
                startRecordingButton.Enabled = true;
                stopRecordingButton.Enabled = false;
            };

            Observer = NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, delegate (NSNotification n)
                {
                    Player.Dispose();
                    Player = null;
                });

            // play recorded sound wireup
            playRecordedSoundButton.TouchUpInside += (sender, e) =>
            {
                try
                {
                    Console.WriteLine("Playing Back Recording " + AudioFilePath.ToString());

                    // The following line prevents the audio from stopping 
                    // when the device autolocks. will also make sure that it plays, even
                    // if the device is in mute
                    AudioSession.Category = AudioSessionCategory.MediaPlayback;

                    Player = new AVPlayer(AudioFilePath);
                    Player.Play();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("There was a problem playing back audio: ");
                    Console.WriteLine(ex.Message);
                }
            };

            #region Layout
            var topPad = (float)NavigationController.NavigationBar.Frame.Size.Height + 40f;

            View.ConstrainLayout(() =>
                image.Frame.Top == View.Frame.Top + topPad &&
                image.Frame.GetCenterX() == View.Frame.GetCenterX() &&
                image.Frame.Height == 100f &&
                image.Frame.Width == 100f &&

                startRecordingButton.Frame.Top == image.Frame.Bottom + 30f &&
                startRecordingButton.Frame.Left == View.Frame.Left + 30f &&
                startRecordingButton.Frame.Height == Constants.ControlsHeight &&
                startRecordingButton.Frame.Width == 100f &&

                recordingStatusLabel.Frame.Top == image.Frame.Bottom + 30f &&
                recordingStatusLabel.Frame.Left == startRecordingButton.Frame.Right + 30f &&
                recordingStatusLabel.Frame.Height == Constants.ControlsHeight &&
                recordingStatusLabel.Frame.Width == 100f &&

                stopRecordingButton.Frame.Top == startRecordingButton.Frame.Bottom + 30f &&
                stopRecordingButton.Frame.Left == View.Frame.Left + 30f &&
                stopRecordingButton.Frame.Height == Constants.ControlsHeight &&
                stopRecordingButton.Frame.Width == 100f &&

                lengthOfRecordingLabel.Frame.Top == startRecordingButton.Frame.Bottom + 30f &&
                lengthOfRecordingLabel.Frame.Left == stopRecordingButton.Frame.Right + 30f &&
                lengthOfRecordingLabel.Frame.Height == Constants.ControlsHeight &&
                lengthOfRecordingLabel.Frame.Width == 100f &&

                playRecordedSoundButton.Frame.Top == stopRecordingButton.Frame.Bottom + 30f &&
                playRecordedSoundButton.Frame.Left == View.Frame.Left + 30f &&
                playRecordedSoundButton.Frame.Height == Constants.ControlsHeight &&
                playRecordedSoundButton.Frame.Width == 100f 
            );
            #endregion
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();

            NSNotificationCenter.DefaultCenter.RemoveObserver(Observer);
        }

        public bool PrepareAudioRecording()
        {
            // You must initialize an audio session before trying to record
            var audioSession = AVAudioSession.SharedInstance();
            var err = audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord);
            if (err != null)
            {
                // Display Alert if needed !!
                return false;
            }
            err = audioSession.SetActive(true);
            if (err != null)
            {
                // Display Alert if needed !!
                return false;
            }

            // Declare string for application temp path and tack on the file extension
            string fileName = string.Format("Myfile{0}.aac", DateTime.Now.ToString("yyyyMMddHHmmss"));
            string tempRecording = Path.Combine(Path.GetTempPath(), fileName);

            Console.WriteLine(tempRecording);
            AudioFilePath = NSUrl.FromFilename(tempRecording);

            //set up the NSObject Array of values that will be combined with the keys to make the NSDictionary
            NSObject[] values = new NSObject[]
            {    
                NSNumber.FromFloat(44100.0f),
                NSNumber.FromInt32((int)AudioToolbox.AudioFormatType.MPEG4AAC),
                NSNumber.FromInt32(1),
                NSNumber.FromInt32((int)AVAudioQuality.High)
            };
            //Set up the NSObject Array of keys that will be combined with the values to make the NSDictionary
            NSObject[] keys = new NSObject[]
            {
                AVAudioSettings.AVSampleRateKey,
                AVAudioSettings.AVFormatIDKey,
                AVAudioSettings.AVNumberOfChannelsKey,
                AVAudioSettings.AVEncoderAudioQualityKey
            };          
            //Set Settings with the Values and Keys to create the NSDictionary
            Settings = NSDictionary.FromObjectsAndKeys(values, keys);

            //Set recorder parameters
            NSError error;
            Recorder = AVAudioRecorder.Create(this.AudioFilePath, new AudioSettings(Settings), out error);
            if ((Recorder == null) || (error != null))
            {
                // Display Alert if needed !!
                return false;
            }

            //Set Recorder to Prepare To Record
            if (!Recorder.PrepareToRecord())
            {
                Recorder.Dispose();
                Recorder = null;
                return false;
            }

            Recorder.FinishedRecording += delegate (object sender, AVStatusEventArgs e)
            {
                Recorder.Dispose();
                Recorder = null;
            };

            return true;
        }
    }
}

