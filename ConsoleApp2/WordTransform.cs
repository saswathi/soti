using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using NetSpell;

namespace Soti_Dict
{
    class WordTransform
    {

        string wsrc;        //start word
        string wdst;        //destination word
        string wtrans;      //transform word
        List<string> wList = new List<string>();
        
        NetSpell.SpellChecker.Dictionary.WordDictionary oDict;
        NetSpell.SpellChecker.Spelling oSpell;

        public string Wsrc
        {
            get { return wsrc; }
        }

        public string Wdst
        {
            get { return wdst; }
        }
        public string Wtrans
        {
            get { return wtrans; }
            set { wtrans = value; }
        }

        public WordTransform()
        {
        }

        public WordTransform(string ws, string wd)
        {
            wsrc = ws;                          //set source word
            wtrans = ws;
            wdst = wd;
            oDict = new NetSpell.SpellChecker.Dictionary.WordDictionary();
            oSpell = new NetSpell.SpellChecker.Spelling();
            oDict.DictionaryFile = "packages\\NetSpell.2.1.7\\dic\\en-US.dic";
            oDict.Initialize();     //set final word
        }

        string ReplaceChar(string s, char c, int p)
        {
            string r;
            var a = s.ToCharArray();
            a[p] = c;            
            r = new string(a);
            return r;
        }

        bool WordFound(string word)
        {
            bool found = true;
            oSpell.Dictionary = oDict;
            if (!oSpell.TestWord(word))
            {
                found = false;
            }
            return found;
        }
        public void doTransform()
        {
            var carr = wdst.ToCharArray();
            int fph = -1;
            bool bLoop = true;
            bool bFound = false;
            string nw = "";
            string pnw = "";
            int pos = 0;
            string wmod = Wtrans;
            string wFirstFound = "";
            int wFirstPos = -1;
            wList.Add(Wtrans);
            while (bLoop)
            {
                
                for(int idx = 0; idx < Wtrans.Length; idx++)
                {
                    //form new word by replacing char at pos in string
                    
                    nw = ReplaceChar(wmod, carr[pos], pos);
                    if(nw == wFirstFound && pos== wFirstPos)
                    {
                        //looped enough, not match
                        bLoop = false;
                        bFound = false;
                        wList.Clear();
                        break;
                    }
                    if (nw != wmod)
                    {
                        bFound = WordFound(nw);
                        if (bFound)
                        {
                            //add word to list
                            wList.Add(nw);

                            //get first placeholder
                            if (fph == -1)
                            {
                                fph = pos;
                            }
                            if(String.IsNullOrEmpty(wFirstFound))
                            {
                                wFirstFound = nw;
                                wFirstPos = pos;
                            }
                            wmod = nw;
                            if(wmod == wdst)
                            {
                                bFound = true;
                                bLoop = false;
                                break;
                            }
                        }
                        else
                        {
                        }
                    }
                    
                    pos++;
                    if(pos>=Wtrans.Length)
                    {
                        pos = 0;
                    }
                }
                if(wmod == Wsrc)
                {
                    bLoop = false;
                    bFound = false;
                    wList.Clear();
                    break;
                }
                if(wmod == Wdst)
                {
                    //transform done;
                    bFound = true;
                    break;
                }
                if(pnw != wmod)
                {
                    pnw = wmod;
                    pos = (fph + 1)>=Wtrans.Length?0: fph + 1;
                }
                else
                {
                    //clear list
                    wList.Clear();
                    wList.Add(Wtrans);
                    wmod = Wtrans;
                    pnw = wmod;
                    pos = (fph + 1) >= Wtrans.Length ? 0 : fph + 1;
                    fph = -1;
                }

                
            }
            if(bFound)
            {
                for(var i=0; i<wList.Count;i++)
                {
                    Console.Write(wList[i]);
                    if(i!=(wList.Count-1))
                    {
                        Console.Write("->");
                    }
                }
                Console.WriteLine(" ");

            }
            else
            {
                Console.WriteLine("Not found right words to transform");
            }
        }
    }
}