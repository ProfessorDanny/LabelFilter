// first LRS Filter program for Scheduling labels
// Basic function to cover printing the labels does not check for change of Doctor
// this was don quickly to cover CBS problem discovered after linux conversion go live



using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LabelFilter
{
    class Program
    {
        private static bool overwrite;

        static int Main(string[] args)
        {

            //Declare any variables in use
            int counter = 0;

            String InputFileName;
            String OutputFileName;
            //String AttrFileName;
            String line;
            String TempFile;
            String term = "\r\n";

            string[] LabelLine = new string[14];
            string ZPLString = "";

            //The filter definition should look like this
            //Datatype all
            //Command - filter.exe (this program)
            //Arguments: &infile &outfile &attrfile

            InputFileName = args[0];
            OutputFileName = args[1];
            //AttrFileName = args[2];


            //InputFileName = @"C:\test\Read4.txt";
            //OutputFileName = @"C:\test\WriteText1.txt";
            //AttrFileName = @"C:\test\AttrFileText.txt";


            //  The following is how to post information to the LRS Printer Log
            //  Console.WriteLine("<!VPSX-MsgType>INFO");
            //  Console.WriteLine("This is what the Info message type will look like"); 

            //  Console.WriteLine("<!VPSX-MsgType>WARN");
            //  Console.WriteLine("This is what the Warning message type will look like");

            //  Console.WriteLine("<!VPSX-MsgType>ERROR");
            //  Console.WriteLine("This is what the Error message type will look like");

            //  Console.WriteLine("<!VPSX-MsgType>DEBUG");
            //  Console.WriteLine("This is what the Debug message type will look like");

            Console.WriteLine("<!VPSX-MsgType>DEBUG");
            Console.WriteLine("Converting Schedule Label TXT to ZPL");

            Console.WriteLine("<!VPSX-MsgType>INFO");
            Console.WriteLine("The Input Filename is {0}", InputFileName);
            Console.WriteLine("<!VPSX-MsgType>INFO");
            Console.WriteLine("The Output Filename is {0} ", OutputFileName);

            // Create a file to write to.


            using (StreamWriter sw = File.CreateText(OutputFileName))
            {
                sw.WriteLine(term);

                Console.WriteLine("<!VPSX-MsgType>DEBUG");
                Console.WriteLine("Create Output File");


            }

            TempFile = string.Format(@"D:\temp\{0}.TXT", Guid.NewGuid());

            File.Copy(InputFileName, TempFile);
            //Console.WriteLine(TempFile);

            //VPSX expects all filters to create an altered output file.  At this point
            //this filter could simply copy the input file to the output file


            // read data from input file
            // Read the file  
            System.IO.StreamReader file =
                new System.IO.StreamReader(TempFile);

            Console.WriteLine("<!VPSX-MsgType>DEBUG");
            Console.WriteLine("Open File for Read");


            while ((line = file.ReadLine()) != null)
            {
                char[] charArr = line.ToCharArray();



                if (line == "")
                {
                    int a;// System.Console.WriteLine(" BLANK Line ");
                }
                else
                {
                    if (line.Length < 2)
                    {
                        //Skip
                        //Console.WriteLine("<!VPSX-MsgType>ERROR");
                        // Console.WriteLine("Short Line Improper Data format Skip Line");
                    }
                    else
                    {
                        //System.Console.WriteLine(line);
                        LabelLine[counter] = line;  // will cause error for wrong data  ie laser job
                        counter++;

                    }


                    if (counter >= 12)
                    {//ERROR
                        Console.WriteLine("<!VPSX-MsgType>ERROR");
                        Console.WriteLine("More than 11 lines with no FF");

                    }

                }


                foreach (char ch in charArr)
                {
                    // if (ch == 0x00)
                    //     System.Console.WriteLine(" NULL ");

                    // if (ch == 0x0A)
                    //     System.Console.WriteLine(" LF ");

                    if (ch == 0x0C) // Form Feed found
                    {

                        // Console.WriteLine("<!VPSX-MsgType>DEBUG");
                        //  Console.WriteLine("Form Feed Found Write One ZPL Label to Output File");


                        //System.Console.WriteLine(" Form Feed ");
                        //NewLabel = 1;  // FF end of label 
                        counter = 0;


                        // Data checks

                        // Start Build ZPL Code for Schedule Label from data


                        // WriteLabel();

                        ZPLString =
                        "^XA" + term +
                        "^LH10,0" + term +
                        "^FT20,38" + term +
                        "^A0N,25,34^FD" + LabelLine[0] + "^FS" + term +
                        "^FT20,68" +
                        "^A0N,25,34^FD" + LabelLine[1] + "^FS" + term +
                       "^FT20,98" +
                        "^A0N,25,34^FD" + LabelLine[2] + "^FS" + term +
                        "^FT20,128" +
                        "^A0N,25,34^FD" + LabelLine[3] + "^FS" + term +
                        "^FT20,158" +
                        "^A0N,25,34^FD" + LabelLine[4] + "^FS" + term +
                        "^FT20,188" +
                        "^A0N,25,34^FD" + LabelLine[5] + "^FS" + term +
                        "^FT20,218" +
                        "^A0N,25,34^FD" + LabelLine[6] + "^FS" + term +
                        "^FT20,248" +
                        "^A0N,25,34^FD" + LabelLine[7] + "^FS" + term +
                        "^FT20,278" +
                        "^A0N,25,34^FD" + LabelLine[8] + "^FS" + term +
                        "^FT20,308" +
                        "^A0N,25,34^FD" + LabelLine[9] + "^FS" + term +
                        "^FT20,338" +
                        "^A0N,25,34^FD" + LabelLine[10] + "^FS" + term +
                        "^FT20,368" +                                       // Added for data error in some formats extra line
                        "^A0N,25,34^FD" + LabelLine[11] + "^FS" + term +    // extra line
                        "^PQ1" +
                        "^XZ" + term + term;
                        // End Schedule Label


                        using (StreamWriter sw = File.AppendText(OutputFileName))
                        {
                            sw.WriteLine(ZPLString);

                        }

                        // zero out label space

                        for (int i = 0; i <= 13; i++)
                        {
                            LabelLine[i] = "";
                        }
                    }

                    // if (ch == 0x0D)
                    //     System.Console.WriteLine(" CR ");

                }



            }

            // close the file?
            file.Close();

            File.Delete(TempFile);

            Console.WriteLine("<!VPSX-MsgType>DEBUG");
            Console.WriteLine("Done Close File and Delete:");

            Console.WriteLine("<!VPSX-MsgType>DEBUG");
            Console.WriteLine(TempFile);

            // System.Console.WriteLine("There were {0} lines.", counter);
            // Suspend the screen.  






            //(StreamWriter w = File.AppendText("log.txt"))

            //VPSX also has Filter Feedback commands which can affect the behavior
            //of VPSX.  I will use those here

            // Console.WriteLine("<!VPSX-DoNotPrint>");  //We don't really want anything printed here
            //Console.WriteLine("<!VPSX-NoOutputFile>"); //Tell VPSX that there is no output file

            return 0;



        }
        /*
        public static void WriteLabel()
        {
            int i;

            ZPLString =
            "^XA" +
            "^LH10,0" +
            "^FT20,38" +
            "^A0N,25,34^FD" + Program.LabelLine[0] + "^FS" +
            "^FT20,68" +
            "^A0N,25,34^FD" + LabelLine[1] + "^FS" +
            "^FT20,98" +
            "^A0N,25,34^FD" + LabelLine[2] + "^FS" +
            "^FT20,128" +
            "^A0N,25,34^FD" + LabelLine[3] + "^FS" +
            "^FT20,158" +
            "^A0N,25,34^FD" + LabelLine[4] + "^FS" +
            "^FT20,188" +
            "^A0N,25,34^FD" + LabelLine[5] + "^FS" +
            "^FT20,218" +
            "^A0N,25,34^FD" + LabelLine[6] + "^FS" +
            "^FT20,248" +
            "^A0N,25,34^FD" + LabelLine[7] + "^FS" +
            "^FT20,278" +
            "^A0N,25,34^FD" + LabelLine[8] + "^FS" +
            "^FT20,308" +
            "^A0N,25,34^FD" + LabelLine[9] + "^FS" +
            "^FT20,338" +
            "^A0N,25,34^FD" + LabelLine[10] + "^FS" +
            "^PQ1" +
            "^XZ" + 0x0C;

           // using (StreamWriter w = File.AppendText(OutputFileName))
          //  {
          //      w.WriteLine(ZPLString);
         //   }

          //  System.IO.File.AppendText


        }
        */
    }



}
