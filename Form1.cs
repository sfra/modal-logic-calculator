using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MA_calc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 frm = new AboutBox1();
            frm.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //int il_atomow = (int)numericUpDown1.Value;






            
            string dictSeq =textBox1.Text;







            string dictSeq2 = textBox2.Text;
            

            int numberOfAtoms=(int)numericUpDown1.Value;


            //       Console.WriteLine("Ile atomów?");
            //     numberOfAtoms = Convert.ToInt32(Console.ReadLine());
            //numberOfAtoms = il_atomow;
            Valuation val = new Valuation(numberOfAtoms);
            Valuation.InterrConn Connn = new Valuation.InterrConn(Program.itIsNecessary);
            Valuation.Val Valuation = new Valuation.Val(Program.valVariables);



           


            try
            {
               

                string beta = textBox3.Text;

                string dictSequence = dictSeq;
               
                string dictSequence_var = dictSeq2;
                

                Dictionary<int, List<int>> Dict =
                    Program.toDictionary(dictSequence);
                Dictionary<string, int[]> Dict_var =
                   Program.toDictionary_str(dictSequence_var);




                Program.setDictNecess(Dict);
                Program.setDictValuaVar(Dict_var);
                Formula fromBeta = Program.ToFormula(beta);
                Set<int> Valua_beta = new Set<int>();
                Valua_beta = val.Valua(Connn, Valuation, fromBeta);
                if(Valua_beta.Elements.Count!=0)
                {
                Program.sort(ref Valua_beta);
                textBox4.Text = Valua_beta.ToString().Substring(0,Valua_beta.ToString().Length-3)+"}";
                }
                else textBox4.Text="{}";


            }
            catch
            {
                MessageBox.Show("Something is wrong with formats");}
                
  			}

        private void openValToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                textBox1.LoadFile(openFileDialog1.FileName);
        }

        private void openInterpretationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
                textBox2.LoadFile(openFileDialog2.FileName);
        }

        private void saveValToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                textBox1.SaveFile(saveFileDialog1.FileName);


        }

        private void saveInterpretationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                textBox2.SaveFile(saveFileDialog2.FileName);
        }
    }
}
