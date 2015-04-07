using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PhotoAndVideoOrganiser;

namespace UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtSourceDirectory.Text =  @"F:\BACKUP FILES\Photos and Video\Photos\2013\2013 02 February";
            txtTargetDirectory.Text = @"F:\BACKUP FILES\Photos and Video\";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var organiser = new PhotoOrganiser(txtTargetDirectory.Text);

            var directories = new List<string>
            {
                txtSourceDirectory.Text
            };

            directories.AddRange(Directory.GetDirectories(txtSourceDirectory.Text, "*.*", SearchOption.AllDirectories));

            foreach (var directory in directories)
            {
                var results = organiser.Organise(directory);

                var numberOfDuplicates = results.Count(x => x.IsDuplicate);
                var numberIncorrect = results.Count(x => x.IsFileNameCorrect == false);
            }
        }
    }
}
