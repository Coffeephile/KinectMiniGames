﻿//------------------------------------------------------------------------------
// <copyright file="KinectAudioViewer.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.WpfViewers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Windows;
    using Microsoft.Kinect;

    /// <summary>
    /// Interaction logic for KinectAudioViewer.xaml
    /// </summary>
    public partial class KinectAudioViewer : KinectViewer
    {
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "ReadOnlyDependencyProperty requires private static field to be initialized prior to the public static field")]
        private static readonly DependencyPropertyKey BeamAnglePropertyKey =
            DependencyProperty.RegisterReadOnly(
                "BeamAngle",
                typeof(double),
                typeof(KinectAudioViewer),
                new PropertyMetadata((double)0));

        public static readonly DependencyProperty BeamAngleProperty = BeamAnglePropertyKey.DependencyProperty;

        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "ReadOnlyDependencyProperty requires private static field to be initialized prior to the public static field")]
        private static readonly DependencyPropertyKey SourceAnglePropertyKey =
            DependencyProperty.RegisterReadOnly(
                "SourceAngle",
                typeof(double),
                typeof(KinectAudioViewer),
                new PropertyMetadata((double)0));

        public static readonly DependencyProperty SourceAngleProperty = SourceAnglePropertyKey.DependencyProperty;

        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "ReadOnlyDependencyProperty requires private static field to be initialized prior to the public static field")]
        private static readonly DependencyPropertyKey ConfidenceLevelPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "ConfidenceLevel",
                typeof(int),
                typeof(KinectAudioViewer),
                new PropertyMetadata(0));

        public static readonly DependencyProperty ConfidenceLevelProperty = ConfidenceLevelPropertyKey.DependencyProperty;
        
        private Stream audioStream;

        public KinectAudioViewer()
        {
            InitializeComponent();
            ViewModelRoot.DataContext = this;
        }

        public double BeamAngle
        {
            get { return (double)GetValue(BeamAngleProperty); }
            private set { SetValue(BeamAnglePropertyKey, value); }
        }
        
        public double SourceAngle
        {
            get { return (double)GetValue(SourceAngleProperty); }
            private set { SetValue(SourceAnglePropertyKey, value); }
        }
        
        public int Confidence
        {
            get { return (int)GetValue(ConfidenceLevelProperty); }
            private set { SetValue(ConfidenceLevelPropertyKey, value); }
        }

        protected override void OnKinectSensorChanged(object sender, KinectSensorManagerEventArgs<KinectSensor> args)
        {
            if (null == args)
            {
                throw new ArgumentNullException("args");
            }

            if ((null != args.OldValue) && (null != args.OldValue.AudioSource))
            {
                // remove old handlers
                args.OldValue.AudioSource.BeamAngleChanged -= AudioSourceBeamChanged;
                args.OldValue.AudioSource.SoundSourceAngleChanged -= AudioSourceSoundSourceAngleChanged;
                
                if (null != audioStream)
                {
                    audioStream.Close();
                }

                args.OldValue.AudioSource.Stop();
            }

            if ((null != args.NewValue) && (null != args.NewValue.AudioSource))
            {
                // add new handlers
                args.NewValue.AudioSource.BeamAngleChanged += AudioSourceBeamChanged;
                args.NewValue.AudioSource.SoundSourceAngleChanged += AudioSourceSoundSourceAngleChanged;

                EnsureAudio(args.NewValue, args.NewValue.Status);
            }
        }

        protected override void OnKinectStatusChanged(object sender, KinectSensorManagerEventArgs<KinectStatus> args)
        {
            if ((null != KinectSensorManager) && (null != KinectSensorManager.KinectSensor) && (null != KinectSensorManager.KinectSensor.AudioSource))
            {
                EnsureAudio(KinectSensorManager.KinectSensor, KinectSensorManager.KinectSensor.Status);
            }            
        }

        protected override void OnAudioWasResetBySkeletonEngine(object sender, EventArgs args)
        {
            if ((null != KinectSensorManager) && (null != KinectSensorManager.KinectSensor) && (null != KinectSensorManager.KinectSensor.AudioSource))
            {
                EnsureAudio(KinectSensorManager.KinectSensor, KinectSensorManager.KinectSensor.Status);
            }
        }

        private void EnsureAudio(KinectSensor sensor, KinectStatus status)
        {
            if ((null != sensor) && (KinectStatus.Connected == status) && sensor.IsRunning)
            {
                audioStream = sensor.AudioSource.Start();
            }
        }

        private void AudioSourceSoundSourceAngleChanged(object sender, SoundSourceAngleChangedEventArgs e)
        {
            // Set width of mark based on confidence
            Confidence = (int)Math.Round(e.ConfidenceLevel * 100);

            // Move indicator
            SourceAngle = e.Angle;
        }

        private void AudioSourceBeamChanged(object sender, BeamAngleChangedEventArgs e)
        {
            // Move our indicator
            BeamAngle = e.Angle;
        }
    }
}
