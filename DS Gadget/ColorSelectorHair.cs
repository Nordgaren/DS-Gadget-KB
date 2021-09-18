﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DS_Gadget
{
    public partial class ColorSelectorHair : Form
    {
        public ColorSelectorHair(DSHook hook)
        {
            InitializeComponent();
            PixelData = (Bitmap)pbxColorSelector.Image;
            Hook = hook;
            R = Hook.HairColorRed;
            G = Hook.HairColorGreen;
            B = Hook.HairColorBlue;
            SetGlowStatus();
        }

        Bitmap PixelData;
        private DSHook Hook;
        private float R;
        private float G;
        private float B;

        private void SetGlowStatus()
        {
            if (Hook.HairColorRed > 1 || Hook.HairColorGreen > 1 || Hook.HairColorBlue > 1)
                cbxGlow.Checked = true;
            else
                cbxGlow.Checked = false;

            if (Hook.EyeColorRed > 1 || Hook.EyeColorGreen > 1 || Hook.EyeColorBlue > 1)
                cbxGlow.Checked = true;
            else
                cbxGlow.Checked = false;
        }

        private void pbxColorSelector_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                var clr = PixelData.GetPixel(e.X, e.Y);
                txtHexColor.Text = $"{clr.R.ToString("X2")}{clr.G.ToString("X2")}{clr.B.ToString("X2")}";
                lblsmallScreen.BackColor = clr;

                if (e.Button == MouseButtons.Left)
                {
                    SetHairColor(clr);
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
        }

        private void SetHairColor(Color clr)
        {
            pnlSelectedScreen.BackColor = clr;
            nudRed.Value = clr.R;
            nudGreen.Value = clr.G;
            nudBlue.Value = clr.B;
        }

        private void pbxColorSelector_MouseDown(object sender, MouseEventArgs e)
        {
            var clr = PixelData.GetPixel(e.X, e.Y);
            SetHairColor(clr);
        }

        private void UpdateTextBox()
        {
            var red = ((byte)nudRed.Value).ToString("X2");
            var green = ((byte)nudGreen.Value).ToString("X2");
            var blue = ((byte)nudBlue.Value).ToString("X2");
            txtHexColor.Text = $"{red}{green}{blue}";
            SetHairColor(Color.FromArgb((byte)nudRed.Value, (byte)nudGreen.Value, (byte)nudBlue.Value));
        }

        private void nudRed_ValueChanged(object sender, EventArgs e)
        {
            Hook.HairColorRed = cbxGlow.Checked ? (float)((nudRed.Value / 255) * 10) : (float)(nudRed.Value / 255);
            if (ActiveControl == sender)
            {
                UpdateTextBox();
            }
        }

        private void nudGreen_ValueChanged(object sender, EventArgs e)
        {
            Hook.HairColorGreen = cbxGlow.Checked ? (float)((nudGreen.Value / 255) * 10) : (float)(nudGreen.Value / 255);
            if (ActiveControl == sender)
            {
                UpdateTextBox();
            }
        }

        private void nudBlue_ValueChanged(object sender, EventArgs e)
        {
            Hook.HairColorBlue = cbxGlow.Checked ? (float)((nudBlue.Value / 255) * 10) : (float)(nudBlue.Value / 255);
            if (ActiveControl == sender)
            {
                UpdateTextBox();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hook.HairColorRed = R;
            Hook.HairColorGreen = G;
            Hook.HairColorBlue = B;
            Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtHexColor_TextChanged(object sender, EventArgs e)
        {
            var color = txtHexColor.Text.PadRight(6, '0');

            var red = byte.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            var green = byte.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            var blue = byte.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            if (ActiveControl == sender)
            {
                var clr = Color.FromArgb(red, green, blue);
                SetHairColor(clr);
            }
            
        }

        private void nud_Leave(object sender, EventArgs e)
        {
            UpdateTextBox();
        }
    }
}
