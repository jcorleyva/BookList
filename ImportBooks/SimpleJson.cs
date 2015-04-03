using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace USATodayBookList.ImportBooks {
    public class SimpleJson {
        private StringBuilder _json= new StringBuilder();
        private Stack<string> _endStack= new Stack<string>();
        private Stack<string> _sepStack= new Stack<string>(); 
        private string _newline= "\n";

        public string Callback {
            set {
                if (String.IsNullOrEmpty(value)) return;
                if (0 < _json.Length || 0 < _endStack.Count)
                    throw new InvalidProgramException("Must set callback before data");
                _json.Append(value);
                Start("(", ")");
            }
        }

        public void Emit(string txt) {
            _json.Append(txt);
        }

        public void Start(string start, string end) {
            Emit(start);
            _newline+= "\t";
            Emit(_newline);
            _endStack.Push(end);
            _sepStack.Push("");
        }

        public static string Quote(string txt) {
            return txt
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n");
        }

        public void EmitQuoted(string data) {
            Emit("\"");
            Emit(Quote(data));
            Emit("\"");
        }

        public void EmitLabel(string label) {
            EmitQuoted(label);
            Emit(":");
        }

        public void EmitSeparator(string separator) {
            Emit(_sepStack.Pop());
            _sepStack.Push(separator+_newline);
        }

        public void End() {
            _newline= _newline.Substring(0, _newline.Length-1);
            Emit(_endStack.Pop());
            _sepStack.Pop();
            Emit(_newline);
        }

        public string Finish() {
            while (_endStack.Count > 0) End();
            return _json.ToString();
        }

        public static SimpleJson ExceptionJson(Exception e) {
            var json= new SimpleJson();
            json.Callback= "ServerError";
            while (e != null) {
                json.Start("{", "}");
                json.EmitLabel("message"); json.EmitQuoted(e.Message);
                json.EmitQuoted("type"); json.EmitQuoted(e.GetType().ToString());
                json.EmitQuoted("stack"); json.EmitQuoted(e.StackTrace);
                e= e.InnerException;
                if (e != null) json.EmitLabel("after");
            }
            return json;
        }
    }
}