namespace SpaceInvadersClone
{
    using System;
    using System.Windows.Forms;

    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new SpaceInvadersForm());
        }
    }
}