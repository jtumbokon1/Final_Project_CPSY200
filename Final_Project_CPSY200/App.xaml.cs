﻿namespace Final_Project_CPSY200
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "Final_Project_CPSY200" };
        }
    }
}
