using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MA_calc
{

    public class Valuation
    {
        private int numberOfAtoms;
        public delegate Set<int> InterrConn(int atom);
        public delegate Set<int> Val(string zm);
        public Set<int> Valua(InterrConn Conn, Val v, Formula alpha)
        {
            Set<int> output = new Set<int>();
            Set<int> unit = new Set<int>();

            for (int i = 1; i <= numberOfAtoms; i++)
            {
                unit._add(i);
            }
            if (alpha.arg == 0) output._copy(v(alpha.variable));
            if (alpha.arg == 1)
            {
                if (alpha.connective == "~") output._copy(unit - Valua(Conn, v, alpha.sbform1));
                if (alpha.connective == "#")
                {
                    for (int i = 0; i < Valua(Conn, v, alpha.sbform1).Elements.Count; i++)
                    {
                        output += Conn(Valua(Conn, v, alpha.sbform1).Elements[i]);

                    }
                }
            }

            if (alpha.arg == 2)
            {

                if (alpha.connective == ">") output._copy((unit - Valua(Conn, v, alpha.sbform1)) + Valua(Conn, v, alpha.sbform2));

                if (alpha.connective == "&") output._copy(Valua(Conn, v, alpha.sbform1) * Valua(Conn, v, alpha.sbform2));






            }


            return output;



        }

        public Valuation(int k)
        { this.numberOfAtoms = k; }


    }








    public class Formula
    {
        public int arg;
        public string connective;
        private int binding;
        public string variable;
        public Formula sbform1;
        public Formula sbform2;



        public override string ToString()
        {
            string output;
            switch (this.arg)
            {
                case 0:
                    output = variable;
                    break;
                case 1:
                    if (sbform1.arg != 0)
                        output = connective + "(" + sbform1.ToString() + ")";
                    else
                        output = connective + sbform1.ToString();
                    break;
                case 2:
                    if (this.binding < sbform1.binding && this.binding < sbform2.binding)
                        output = "(" + sbform1.ToString() + ")" + connective + "(" + sbform2.ToString() + ")";
                    else if (this.binding >= sbform1.binding && this.binding < sbform2.binding)
                        output = sbform1.ToString() + connective + "(" + sbform2.ToString() + ")";
                    else if (this.binding < sbform1.binding && this.binding >= sbform2.binding)
                        output = "(" + sbform1.ToString() + ")" + connective + sbform2.ToString();
                    else
                        output = sbform1.ToString() + connective + sbform2.ToString();
                    break;
                default:
                    output = "Not a formula";
                    break;
            }
            return output;
        }


        public Formula(string p)
        {
            arg = 0;
            variable = p;
            binding = 0;
        }
        public Formula(string sp, Formula a)
        {
            arg = 1;
            connective = sp;
            sbform1 = a;
            binding = 1;
        }
        public Formula(string sp, Formula a, Formula b)
        {
            arg = 2;
            connective = sp;
            sbform1 = a;
            sbform2 = b;
            if (sp == "&") binding = 2;
            else binding = 3;
        }


    }





    public class Set<typ>// : IComparable<Set<typ>>
    {

        private List<typ> elements;
        public Set()
        {
            elements = new List<typ>();
        }



        public Set(typ[] tab)
        {
            elements = new List<typ>();
            foreach (typ t in tab)
            {
                elements.Add(t);

            }

        }





        public List<typ> Elements
        {
            get
            { return elements; }
            set
            { elements = value; }
        }



        public void _copy(Set<typ> prev)
        {
            this.elements = new List<typ>(prev.elements);
        }




       // public int numberOfElements = 0;
        public override string ToString()
        {

            //Program.sort(ref this);
            string output = "{";
            if (this.elements.Count == 0) return "{}";


            for (int i = 0; i < elements.Count; i++)
            {
                output += elements[i] + ",";
            }

            return output + "\b}";
        }



        public void _add(typ element)
        {
            if (!elements.Contains(element))
            {
                elements.Add(element);
            }

        }

        public static Set<typ> operator +(Set<typ> a, Set<typ> b)
        {

            Set<typ> output = new Set<typ>();

            output._copy(a);

            for (int i = 0; i < b.elements.Count; i++)
                if (!(a.elements.Contains(b.elements[i]))) output._add(b.elements[i]);


            return output;
        }



        public static Set<typ> operator *(Set<typ> a, Set<typ> b)
        {

            Set<typ> output = new Set<typ>();



            for (int i = 0; i < b.elements.Count; i++)
                if ((a.elements.Contains(b.elements[i]))) output._add(b.elements[i]);

            return output;
        }

        public static Set<typ> operator -(Set<typ> a, Set<typ> b)
        {

            Set<typ> output = new Set<typ>();



            for (int i = 0; i < a.elements.Count; i++)
                if (!(b.elements.Contains(a.elements[i]))) output._add(a.elements[i]);

            return output;
        }



    }




    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 


        public static void sortRec(int begin, ref Set<int> a, int end)
        {

            int replacement;
            int j;
            List<int> table = a.Elements;
            int i = (begin + end) / 2;
            int pivot = table[i];
            replacement = table[i];
            table[i] = table[end];
            table[end] = replacement;

            for (j = i = 0; i < end; i++)

                if (table[i] < pivot)
                {
                    replacement = table[i];
                    table[i] = table[j];
                    table[j] = replacement;
                    j++;
                }

            table[end] = table[j];
            table[j] = pivot;


            if (begin < j - 1) sortRec(begin, ref a, j - 1);
            if (j + 1 < end) sortRec(j + 1, ref a, end);
        }




        public static void sort(ref Set<int> a)
        {

            if (a.Elements.Count > 0)

                sortRec(0, ref a, a.Elements.Count - 1);
        }


        public static Set<int> toSet(string s)
        {

            Set<int> output = new Set<int>();

            try
            {

                if (s[0] == '{' && s[s.Length - 1] == '}')
                {
                    char[] splitters = { ',' };
                    foreach (string subSequence in (s.Substring(1, s.Length - 2)).Split(splitters))
                    {
                        output._add(Convert.ToInt32(subSequence));

                    }
                }
            }
            catch (System.FormatException)
            { }

            Console.WriteLine("Something wrong with the format of your set");
            return output;

        }


		//potrzebna metoda product?
        static Formula product(Formula x, Formula y)
        {
            Formula z = new Formula("&", x, y);
            return z;
        }

        static int polar(string s, int place)
        {
            int output = 0;
            for (int i = 0; i < place; i++)
            {
                if (s[i] == '(') output--;
                else if (s[i] == ')') output++;

            }
            return output;
        }


        static bool isComplex(string s)
        {

            for (int k = 0; k < s.Length; k++)
            {
                if (isLogical(s[k]) && s[k] != '(' && s[k] != ')') return true;
            }
            return false;

        }



        static int mainConnective(string s, int start)
        {
            for (int i = start; i < s.Length - start; i++)
            {

                if (!isComplex(s)) return -1;
                if (polar(s, start + i) == -1)
                {
                    if (s[start + i] == '(') return start + i - 1;
                    else
                        return start + i;
                }
            }
            return -1;
        }


        static bool isLogical(char c)
        {
            return (c == '(') || (c == ')') || (c == '>') || (c == '~') || (c == '#') || (c == '&');

        }


        static void enclVariable(ref string s)
        {
            for (int k = (s.Length - 1); 0 <= k; k--)
            {


            begin_it:
                if (!isLogical(s[k]))
                {

                    for (int i = k; 0 <= i; i--)
                    {

                        if ((isLogical(s[i]) || i == 0) && ((s[i] != '(') || (s[k + 1] != ')')))
                        {
                            s = s.Insert(k + 1, ")");

                            if (i == 0) s = s.Insert(0, "(");
                            else
                                s = s.Insert(i + 1, "(");

                            k = i;
                            goto begin_it;
                        }
                        else if ((isLogical(s[i]) || i == 0) && !((s[i] != '(') || (s[k + 1] != ')'))) { k = i; break; }

                    }
                }
            }
        }

        static void enclNegation(ref string s)
        {
            for (int k = (s.Length - 1); 0 <= k; k--)
            {



                if (s[k] == '~' || s[k] == '#')
                {
                    int j = 1;



                    while (polar(s.Substring((k + 1), j), j) != 0)
                    {
                        j++;

                    }
                    if (k == 0 || s[k - 1] != '(' || s[k + j + 1] != ')')
                    {
                        s = s.Insert(k + j + 1, ")");
                        s = s.Insert(k, "(");
                        j = 0;
                    }
                }
            }
        }


        static void enclConnective(ref string s, char spj)
        {
            for (int k = (s.Length - 1); 0 <= k; k--)
            {


                if (s[k] == spj)
                {
                    int j = 1;



                    while (polar(s.Substring((k + 1), j), j) != 0)
                    {
                        j++;

                    }

                    int i = 1;

                    while (polar(s.Substring((k - i), i), i) != 0)
                    {
                        i++;

                    }
                    if (k - i - 1 == 0 || s[k - i - 1] != '(' || s[k + j + 1] != ')')
                    {
                        s = s.Insert(k + j + 1, ")");
                        s = s.Insert(k - i, "(");
                        j = 0; i = 1;
                    }

                }
            }
        }


        static string enclFormula(string s)
        {
            string plc = s;

            plc = plc + ")";
            plc = plc.Insert(0, "(");
            if (!isComplex(s)) return plc;
            enclVariable(ref plc);
            enclNegation(ref plc);
            enclConnective(ref plc, '&');
            enclConnective(ref plc, '>');
            if (mainConnective(plc, 1) == 1) return plc;
            return plc.Substring(1, plc.Length - 2);
        }


        public static Formula ToFormula(string s)
        {
            if (isComplex(s)) s = enclFormula(s);
            int k = mainConnective(s, 1);
            int j = 1;
            if (k == -1)
                if (s[0] == '(') return new Formula(s.Substring(1, s.Length - 2));
                else return new Formula(s);



            while (polar(s.Substring((k + 1), j), j) != 0) j++;
            if (s[k] == '~' || s[k] == '#') return new Formula(Convert.ToString(s[k]), ToFormula(s.Substring(k + 1, j)));
            int i = 1;

            while (polar(s.Substring((k - i), i), i) != 0) i++;



            return new Formula(s[k].ToString(),
                            ToFormula(s.Substring((k - i), i)),
                                ToFormula(s.Substring(k + 1, j)));


        }


     /*   public static List<int> vvart;//={1,2,4};
        //   public static int[,] vvartx2;
        public static void nadaj(List<int> a)
        {
            vvart = a;
        }

*/


        public static Dictionary<int, List<int>> DictNecess;

        public static void setDictNecess(Dictionary<int, List<int>> Sl)
        {

            DictNecess = Sl;

        }


        //a to jest interpratacja endzności
        public static Set<int> itIsNecessary(int a)
        {
            List<int> vart = DictNecess[a];


            Set<int> output = new Set<int>();
            output.Elements = vart;
            return output;

        }

        //nadajemy Valuaości zmiennym zdaniowym



        public static Dictionary<string, int[]> dictValuaVar;

        public static void setDictValuaVar(Dictionary<string, int[]> Sl)
        {
            dictValuaVar = Sl;

        }

        public static Set<int> valVariables(string s)
        {
            int[] vart = dictValuaVar[s];

            Set<int> output = new Set<int>(vart);
            return output;



        }

        public static Dictionary<int, List<int>> toDictionary(string s)
        {
            Dictionary<int, List<int>> output = new Dictionary<int, List<int>>();
            char[] splitters = { '\n' };
            char[] splitters2 = { ',' };
            string[] results = s.Split(splitters);
            string[] results2;
      //      char kontrol_s = s[s.Length - 1];
         //   if (s[s.Length - 1] == ':') { return output; }

            for (int i = 0; i < results.Length; i++)
            {
                List<int> codom = new List<int>();
                int colonIndex = results[i].IndexOf(":");
                 
                string head = (results[i]).Substring(0, colonIndex);
                string body = (results[i]).Substring(colonIndex + 1, (results)[i].Length - colonIndex - 1);
               // if (body.Length == 0) { }
                
                results2 = body.Split(splitters2);

                if (results[i].Length != colonIndex + 1)
                {
                       foreach (string w in results2)
                    {
                        codom.Add(Convert.ToInt32(w));
                    }
                }


                output.Add(Convert.ToInt32(head), codom);
            }

            return output;

        }










        public static Dictionary<string, int[]> toDictionary_str(string s)
        {
            Dictionary<string, int[]> output = new Dictionary<string, int[]>();
            char[] splitters = { '\n' };
            char[] splitters2 = { ',' };
            string[] results = s.Split(splitters);
            string[] results2;
            
          //  if (s[s.Length - 1] == ':') { return output; }
            for (int i = 0; i < results.Length; i++)
            {

                int colonIndex = results[i].IndexOf(":");
                string head = (results[i]).Substring(0, colonIndex);
                string body = (results[i]).Substring(colonIndex + 1, (results)[i].Length - colonIndex - 1);
                results2 = body.Split(splitters2);
                int[] codom = new int[results2.Length];

                if (results[i].Length != colonIndex + 1)
                {

                    for (int k = 0; k < results2.Length; k++)// string w in results2)
                    {
                        codom[k] = Convert.ToInt32(results2[k]);
                    }

                }
                else
                { int[] ccodom = new int[] { };
                output.Add(head, ccodom);
                continue;
                }

                
                    /**/
                
                 
                /**/


                output.Add(head, codom);
            }

            return output;

        }




        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
