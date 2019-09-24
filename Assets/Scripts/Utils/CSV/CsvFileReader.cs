using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Indigo.Core.Util;

namespace Utils.CSV
{
    /// <summary>
    ///     Class for reading from comma-separated-value (CSV) files
    /// </summary>
    public class CsvFileReader : CsvFileCommon, IDisposable
    {
        private string _currLine;
        private int _currPos;

        private readonly EmptyLineBehavior _emptyLineBehavior;

        // Private members
        private readonly StreamReader _reader;

        /// <summary>
        ///     Initializes a new instance of the CsvFileReader class for the
        ///     specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <param name="emptyLineBehavior">Determines how empty lines are handled</param>
        public CsvFileReader (StreamReader reader,
            EmptyLineBehavior emptyLineBehavior = EmptyLineBehavior.NO_COLUMNS)
        {
            _reader = reader;
            _emptyLineBehavior = emptyLineBehavior;
        }

        /// <summary>
        ///     Initializes a new instance of the CsvFileReader class for the
        ///     specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <param name="emptyLineBehavior">Determines how empty lines are handled</param>
        public CsvFileReader(Stream stream,
            EmptyLineBehavior emptyLineBehavior = EmptyLineBehavior.NO_COLUMNS)
        {
            _reader = new StreamReader(stream);
            _emptyLineBehavior = emptyLineBehavior;
        }

        /// <summary>
        ///     Initializes a new instance of the CsvFileReader class for the
        ///     specified file path.
        /// </summary>
        /// <param name="path">The name of the CSV file to read from</param>
        /// <param name="emptyLineBehavior">Determines how empty lines are handled</param>
        public CsvFileReader(string path,
            EmptyLineBehavior emptyLineBehavior = EmptyLineBehavior.NO_COLUMNS)
        {
            _reader = new StreamReader(path);
            _emptyLineBehavior = emptyLineBehavior;
        }

        // Propagate Dispose to StreamReader
        public void Dispose()
        {
            _reader.Dispose();
        }

        public static List<List<string>> ReadAll(string path, Encoding encoding)
        {
            using (var sr = new StreamReader(path, encoding))
            {
                var cfr = new CsvFileReader(sr);
                var dataGrid = new List<List<string>>();
                if (cfr.ReadAll(dataGrid))
                {
                    return dataGrid;
                }
            }
            return null;
        }

        public bool ReadAll(List<List<string>> dataGrid)
        {
            // Verify required argument
            if (dataGrid == null)
            {
                throw new ArgumentNullException("dataGrid");
            }

            var row = new List<string>();
            while (ReadRow(row))
            {
                dataGrid.Add(new List<string>(row));
            }

            return true;
        }

        /// <summary>
        ///     Reads a row of columns from the current CSV file. Returns false if no
        ///     more data could be read because the end of the file was reached.
        /// </summary>
        /// <param name="columns">Collection to hold the columns read</param>
        public bool ReadRow(List<string> columns)
        {
            // Verify required argument
            if (columns == null)
            {
                throw new ArgumentNullException("columns");
            }

            ReadNextLine:
            // Read next line from the file
            _currLine = _reader.ReadLine();
            _currPos = 0;
            // Test for end of file
            if (_currLine == null)
            {
                return false;
            }
            // Test for empty line
            if (_currLine.Length == 0)
            {
                switch (_emptyLineBehavior)
                {
                    case EmptyLineBehavior.NO_COLUMNS:
                        columns.Clear();
                        return true;
                    case EmptyLineBehavior.IGNORE:
                        goto ReadNextLine;
                    case EmptyLineBehavior.END_OF_FILE:
                        return false;
                }
            }

            // Parse line
            string column;
            var numColumns = 0;
            while (true)
            {
                // Read next column
                if (_currPos < _currLine.Length && _currLine[_currPos] == Quote)
                {
                    column = ReadQuotedColumn();
                }
                else
                {
                    column = ReadUnquotedColumn();
                }
                // Add column to list
                if (numColumns < columns.Count)
                {
                    columns[numColumns] = column;
                }
                else
                {
                    columns.Add(column);
                }
                numColumns++;
                // Break if we reached the end of the line
                if (_currLine == null || _currPos == _currLine.Length)
                {
                    break;
                }
                // Otherwise skip delimiter
                Debug.Assert(_currLine[_currPos] == Delimiter);
                _currPos++;
            }
            // Remove any unused columns from collection
            if (numColumns < columns.Count)
            {
                columns.RemoveRange(numColumns, columns.Count - numColumns);
            }
            // Indicate success
            return true;
        }

        /// <summary>
        ///     Reads a quoted column by reading from the current line until a
        ///     closing quote is found or the end of the file is reached. On return,
        ///     the current position points to the delimiter or the end of the last
        ///     line in the file. Note: CurrLine may be set to null on return.
        /// </summary>
        private string ReadQuotedColumn()
        {
            // Skip opening quote character
            Debug.Assert(_currPos < _currLine.Length && _currLine[_currPos] == Quote);
            _currPos++;

            // Parse column
            var builder = new StringBuilder();
            while (true)
            {
                while (_currPos == _currLine.Length)
                {
                    // End of line so attempt to read the next line
                    _currLine = _reader.ReadLine();
                    _currPos = 0;
                    // Done if we reached the end of the file
                    if (_currLine == null)
                    {
                        return builder.ToString();
                    }
                    // Otherwise, treat as a multi-line field
                    builder.Append(Environment.NewLine);
                }

                // Test for quote character
                if (_currLine[_currPos] == Quote)
                {
                    // If two quotes, skip first and treat second as literal
                    var nextPos = _currPos + 1;
                    if (nextPos < _currLine.Length && _currLine[nextPos] == Quote)
                    {
                        _currPos++;
                    }
                    else
                    {
                        break; // Single quote ends quoted sequence
                    }
                }
                // Add current character to the column
                builder.Append(_currLine[_currPos++]);
            }

            if (_currPos < _currLine.Length)
            {
                // Consume closing quote
                Debug.Assert(_currLine[_currPos] == Quote);
                _currPos++;
                // Append any additional characters appearing before next delimiter
                builder.Append(ReadUnquotedColumn());
            }
            // Return column value
            return builder.ToString();
        }

        /// <summary>
        ///     Reads an unquoted column by reading from the current line until a
        ///     delimiter is found or the end of the line is reached. On return, the
        ///     current position points to the delimiter or the end of the current
        ///     line.
        /// </summary>
        private string ReadUnquotedColumn()
        {
            var startPos = _currPos;
            _currPos = _currLine.IndexOf(Delimiter, _currPos);
            if (_currPos == -1)
            {
                _currPos = _currLine.Length;
            }
            if (_currPos > startPos)
            {
                return _currLine.Substring(startPos, _currPos - startPos);
            }
            return string.Empty;
        }
    }
}