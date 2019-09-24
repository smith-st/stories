using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Indigo.Core.Util;
using UnityEngine;

namespace Utils.CSV
{
	
    public class CSVDataTable
    {
		List<List<string>> _allText;

		List<string> _header;
		public List<string> Header
        {
            get
            {
                return _header;
            }
        }
		int _rowCount;									
		public 	int RowCount
        {
            get
            {
                return _rowCount;
            }
        }

		public CSVDataTable()
        {
            _allText = new List<List<string>> ();
            _header = new List<string> ();
        }


		public List<string> GetRow (int index)
        {

            if (_rowCount > index && index >= 0)
            {
                return _allText.ElementAt (index);
            } else
            {
                throw(new ArgumentOutOfRangeException ());
            }

				
        }


		public void ReadFromStream(Stream stream)
        {
            try
            {
                using (var reader = new CsvFileReader (stream, EmptyLineBehavior.IGNORE))
                {
                    var dataGrid = new List<List<string>> ();
                  
                    if (reader.ReadAll (dataGrid))
                    {
                        _header = dataGrid[0];
                        dataGrid.RemoveAt (0);
                        _allText = dataGrid;
                        _rowCount = _allText.Count;
                        for (int i = 0; i < _allText.Count; ++i)
                        {
                            var it = _allText[i];
                            if (it.Count != _header.Count)
                            {
                                // +2 - in all spreadsheets rows start from 1 and first line is a header
                                throw new Exception(string.Format("Row on line {0} contains different amount of collumns than header ({1} vs {2}", i + 2, it.Count, _header.Count)); 
                            }
                        }
                    } else
                    {
                        throw(new Exception("CSV table read failed"));
                    }
                }
            } 
            catch(IOException e)
            {
                throw new Exception(string.Format("IO error: {0}", e));
            }
		
        }

                          
		public void ReadFromTextAsset(TextAsset textAsset)
        {
            using (var stream = new MemoryStream (textAsset.bytes))
            {
                ReadFromStream (stream);
            }
        }
    }

}