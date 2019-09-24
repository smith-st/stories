using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utils.CSV
{
    /// <summary>
    ///     Class for writing to comma-separated-value (CSV) files.
    /// </summary>
    public class CsvFileWriter : CsvFileCommon, IDisposable
    {
        private string _oneQuote;
        private string _quotedFormat;

        private string _twoQuotes;

        // Private members
        private readonly StreamWriter _writer;

        /// <summary>
        ///     Initializes a new instance of the CsvFileWriter class for the
        ///     specified stream.
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        public CsvFileWriter(StreamWriter writer)
        {
            _writer = writer;
        }

        /// <summary>
        ///     Initializes a new instance of the CsvFileWriter class for the
        ///     specified stream.
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        public CsvFileWriter(Stream stream)
        {
            _writer = new StreamWriter(stream);
        }

        /// <summary>
        ///     Initializes a new instance of the CsvFileWriter class for the
        ///     specified file path.
        /// </summary>
        /// <param name="path">The name of the CSV file to write to</param>
        public CsvFileWriter(string path)
        {
            _writer = new StreamWriter(path);
        }

        // Propagate Dispose to StreamWriter
        public void Dispose()
        {
            _writer.Dispose();
        }

        public static void WriteAll(List<List<string>> dataGrid, string path, Encoding encoding)
        {
            using (var sw = new StreamWriter(path, false, encoding))
            {
                var cfw = new CsvFileWriter(sw);
                foreach (var row in dataGrid)
                {
                    cfw.WriteRow(row);
                }
            }
        }

        public void WriteAll(List<List<string>> dataGrid)
        {
            foreach (var row in dataGrid)
            {
                WriteRow(row);
            }
        }

        /// <summary>
        ///     Writes a row of columns to the current CSV file.
        /// </summary>
        /// <param name="columns">The list of columns to write</param>
        public void WriteRow(List<string> columns)
        {
            // Verify required argument
            if (columns == null)
            {
                throw new ArgumentNullException("columns");
            }

            // Ensure we're using current quote character
            if (_oneQuote == null || _oneQuote[0] != Quote)
            {
                _oneQuote = string.Format("{0}", Quote);
                _twoQuotes = string.Format("{0}{0}", Quote);
                _quotedFormat = string.Format("{0}{{0}}{0}", Quote);
            }

            // Write each column
            for (var i = 0; i < columns.Count; i++)
            {
                // Add delimiter if this isn't the first column
                if (i > 0)
                {
                    _writer.Write(Delimiter);
                }
                // Write this column
                if (columns[i].IndexOfAny(SpecialChars) == -1)
                {
                    _writer.Write(columns[i]);
                }
                else
                {
                    _writer.Write(_quotedFormat, columns[i].Replace(_oneQuote, _twoQuotes));
                }
            }
            _writer.Write("\r\n");
        }
    }
}