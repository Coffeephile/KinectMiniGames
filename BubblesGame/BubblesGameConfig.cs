﻿using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using System.ComponentModel;

namespace BubblesGame
{
    public class BubblesGameConfig : INotifyPropertyChanged
    {
        #region Private State
        private KinectSensorChooser kinectSensor;
        private string username = "Gracz";
        private int bubbleSize = 40;
        private int bubblesCount = 20;
        private int bubbleFallSpeed = 3;
        private int bubblesApperanceFrequency = 2;
        #endregion

        #region Public State
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }

        public int BubblesFallSpeed
        {
            get { return bubbleFallSpeed; }
            set
            {
                bubbleFallSpeed = value;
                OnPropertyChanged("BubblesFallSpeed");
            }
        }

        public int BubblesCount
        {
            get { return bubblesCount; }
            set
            {
                bubblesCount = value;
                OnPropertyChanged("BubblesCount");
            }
        }

        public int BubblesSize
        {
            get { return bubbleSize; }
            set
            {
                bubbleSize = value;
                OnPropertyChanged("BubblesSize");
            }
        }

        public int BubblesApperanceFrequency
        {
            get { return bubblesApperanceFrequency; }
            set
            {
                bubblesApperanceFrequency = value;
                OnPropertyChanged("BubblesApperanceFrequency");
            }
        }

        public KinectSensorChooser PassedKinectSensorChooser
        {
            get { return kinectSensor; }
            set { kinectSensor = value; }
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propname));
            }
        }

        #endregion
    }
}
