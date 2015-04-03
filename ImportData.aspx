<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportData.aspx.cs" Inherits="USATodayBookList.ImportData" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
            table {border-collapse: collapse}
            td, th {border: 1px solid; padding: 2px}
            input[type=button] {
                color:#333333;
            }
            em input[type=button] {color:black;font-weight:bolder;}
        </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="black">
        <p>
            <div id="Status" style="height: 20px">
                Checking database...
            </div>
        </p>
        <div id="Controls">
        </div>
        <div id="Data">
            
        </div>
    </div>
    <script type="text/javascript">
        function $(id) {
            return document.getElementById(id);
        }
        function quoteHtml(txt) {
            return txt
                .replace(/[&]/g, '&amp;')
                .replace(/[<]/g, '&lt;')
                .replace(/[>]/g, '&gt;')
                .replace(/["]/g, '&quot;');
        }
        function setStatus(txt) {
            $('Status').innerHTML=txt;
        }
        function noCache() {
            return 'cachedefeat='+new Date().getTime();
        }
        function doServer(cmd, succeed, fail) {
            var img=new Image();
            img.setAttribute('width', 1);
            img.setAttribute('height', 1);
            if (img.addEventListener) {
            	img.addEventListener('load', succeed);
            	img.addEventListener('abort', fail);
            	img.addEventListener('error', fail);
	    } else {
            	img.onload= succeed;
            	img.onabort= fail;
            	img.onerror= fail;
	    }
            img.setAttribute('src', 'ImportBooks/'+cmd+noCache());
            document.getElementsByTagName('head')[0].appendChild(img);
        }
        function getServer(cmd) {
            var script=document.createElement('script');
            script.setAttribute('src', 'ImportBooks/'+cmd+noCache());
            script.setAttribute('type', 'text/javascript');
            document.getElementsByTagName('head')[0].appendChild(script);
        }
        function setMondays(obj) {
            if (!obj||!obj.mondays) alert('server error: server hates mondays');
            var M=obj.mondays;
            setStatus('Please select date to process: ');
            var t=['<ul>\n'];
            for (var j=0; j<M.length; j++) {
                var label=M[j].MonDayYear;
                var date=M[j].isodate;
                var current=M[j].ThisWeek;
                if (current) t.push('<em>');
                t.push('<input type="button" value="');
                t.push(label)
                t.push('" onClick="mondayIs(\'')
                t.push(date)
                t.push('\', \'');
                t.push(label);
                t.push('\')"> &nbsp; <input type="button" value="Delete ');
                t.push(label);
                t.push('" onclick="doDelete(\'')
                t.push(date);
                t.push('\')"/>');
                if (current) t.push('</em>');
                t.push('<br />\n');
            }
            t.push('</ul>');
            $('Controls').innerHTML=t.join('');
        }
        validateStatus='Something has gone wrong -- you should never see this message...';
        function mondayIs(dt, lbl) {
            $('Controls').innerHTML='';
            validateStatus='Validating input files for '+lbl+'...';
            setStatus(validateStatus);
            doServer('setcurrentweek.ashx?monday='+dt+'&', doneValidate, failValidate);
        }
        function doValidate() {
            $('Controls').innerHTML= '';
            setStatus(validateStatus);
            doServer('validate.ashx?', doneValidate, failValidate);
        }
        function doneValidate() {
            setStatus(validateStatus+'done');
            $('Controls').innerHTML='...';
            getServer('importlog.ashx?callback=showImportReady&');
        }
        function failValidate() {
            setStatus(validateStatus+'failed');
        }
        function enableImport() {
            var B1='<input type="button" value="Re-Validate" id="CmdValidate" onclick="doValidate()" />';
            var B2='<input type="button" value="Import" id="CmdImport" onclick="doImport()" />';
            var r=B1+'<br />\n'+B2+'\n';
            $('Controls').innerHTML=r;
            return r;
        }
        var _keepMonitoring=false;
        var _monitorCount=0;
        function doImport() {
            setStatus('Importing...');
            doServer('import.ashx?', doneImport, failImport);
            monitorImport(true);
        }
        function doneImport() {
            setStatus('Importing...done');
            monitorImport(false);
        }
        function failImport() {
            setStatus('Importing...failed');
            clearInterval(_importMonitor);
            monitorImport(false);
        }
        function monitorImport(more) {
            if (arguments.length)
                _keepMonitoring=more;
            if (300<_monitorCount++) {
                setStatus('Server timeout!');
                _keepMonitoring=false;
            }
            getServer('importlog.ashx?callback=showImportLog&');
            if (_keepMonitoring)
                setTimeout("monitorImport()", 1000);
        }
        function showImportReady(obj) {
            showImportLog(obj);
            enableImport();
        }
        function showImportLog(obj) {
            if (!obj.importLog) {
                alert("invalid server response: no providers");
                return;
            }
            var tbl=obj.importLog;
            var cols='ProviderName CurrentImportFile FileExtension RowsImported Bad'.split(' ');
            var t=['<table><tr><th>', 'Provider File Ext Rows Bad'.replace(/ /g, '</th><th>')];
            for (var j=0; j<tbl.length; j++) {
                var r=[], row=tbl[j];
                var er=row.ErrorText;
                t.push('</td></tr><tr>');
                var td=(!er)?'<td>':'<td class="error" title="'+quoteHtml(er)+'">';
                t.push(td);
                for (var k=0; k<cols.length; k++)
                    r.push(row[cols[k]]);
                t.push(r.join('</td>'+td));
            }
            t.push('</td></tr></table>');
            $('Data').innerHTML=t.join('');
        }
        function doDelete(dt) {
            if (confirm('Are you sure you want to delete everything that has been imported for this week ('+dt+')?')) {
                setStatus('Deleting '+dt+' ...');
                doServer('deleteThisWeeksData.ashx?areyousure=1&week='+dt+'&', function(){doneDelete(dt);}, function(){failDelete(dt);});
            }
        }
        function doneDelete(dt) {
            getServer('importlog.ashx?callback=showImportLog&');
            <%= USATodayBookList.ImportBooks.Mondays.GetMondays("setMondays") %>
            setStatus('Deleting '+dt+' ...done');
        }
        function failDelete(dt) {
            setStatus('Deleting '+dt+' ...failed');
        }
	setTimeout(function(){
        <%= USATodayBookList.ImportBooks.Mondays.GetMondays("setMondays") %>
	}, 1);
        <%= USATodayBookList.ImportBooks.ImportLog.GetImportLog("showImportLog") %>
    </script>
</asp:Content>
