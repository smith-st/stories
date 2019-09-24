using System;
using System.Collections.Generic;
using System.Linq;

public class ScenarioStorage {
    private Dictionary<string, int> _header; 
    private Dictionary<string, List<string>> _rows;
    private int _firstSceneKey = int.MaxValue;

    public string FirstKey {
        get { return _firstSceneKey.ToString(); }
    }
    
    public Dictionary<string, int> Header {
        get { return _header; }
    }
    
    public void RegisterHeader(List<string> header) {
        _header = new Dictionary<string, int>(header.Count);
        for (var i = 0; i < header.Count; i++) {
            _header.Add(header[i],i);
        }
        _rows = new Dictionary<string, List<string>>();
    }
    
    public void RegisterRow(List<string> row) {
        int key; 
        if(int.TryParse(row[_header["key"]], out key)) {
            if (key < _firstSceneKey)
                _firstSceneKey = key;
        }
        _rows.Add(row[_header["key"]],row);
    }
    
    public Dictionary<string,string> GetRow(string key) {
        if (!_rows.ContainsKey(key))
            return null;
        var row = new Dictionary<string,string>(_header.Count);
        foreach (var item in _header) {
            row.Add(item.Key,_rows[key][item.Value]);
        }
        return row;
    }
    
    public string NextKeyAfter(string key) {
        var next = false;
        foreach (var row in _rows) {
            if (next) {
                return row.Key;
            }
            if (row.Key == key) {
                next = true;
            }
        }

        return "";
    }
    
    
}