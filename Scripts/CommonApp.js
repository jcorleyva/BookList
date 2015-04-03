// JScript File
//---------------------------

//---------------------------
function Scroller()
{
}
Scroller.GetCoords = function()
{
    var scrollX, scrollY;
    if (document.all)
    {
        if (!document.documentElement.scrollLeft)
            scrollX = document.body.scrollLeft;
        else
            scrollX = document.documentElement.scrollLeft;
        if (!document.documentElement.scrollTop)
            scrollY = document.body.scrollTop;
        else
            scrollY = document.documentElement.scrollTop;
    }
    else
    {
        scrollX = window.pageXOffset;
        scrollY = window.pageYOffset;
    }

    document.getElementById('_hdnScrollLeft_').value = scrollX;
    document.getElementById('_hdnScrollTop_').value = scrollY;
}
Scroller.Scroll = function()
{
    var x = document.getElementById('_hdnScrollLeft_').value;
    var y = document.getElementById('_hdnScrollTop_').value;
    var i = 0;
    while (i++ < 100 && (document.body.scrollLeft != x || document.body.scrollTop != y))
        window.scrollTo(x, y);
}
Scroller.ScrollToTop = function(bParent/*=false*/)
{
    if (bParent == true)
        window.parent.scrollTo(0, 0);
    else
        window.scrollTo(0, 0);
}

//---------------------------
function DataType()
{
}
DataType.IsEmpty = function(value)
{
    return (typeof (value) == "undefined" || value == null || (typeof (value) == 'string' && value == ''))
}
DataType.IsBoolean = function(value)
{
    return typeof (value) == 'boolean';
}
DataType.IsArray = function(value)
{
    return typeof (value) == 'object' && !DataType.IsEmpty(value["length"]);
}
DataType.ToString = function(str, defaultValue/*=""*/)
{
    if (DataType.IsEmpty(str))
    {
        return (DataType.IsEmpty(defaultValue) ? "" : defaultValue.toString());
    }
    else
        return str.toString();
}
DataType.ToBool = function(value, defaultValue/*=""*/)
{
    if (!DataType.IsBoolean(defaultValue)) defaultValue = false;

    return value == !defaultValue ? !defaultValue : defaultValue;
}
DataType.ToBoolString = function(value, defaultValue/*=""*/)
{
    return DataType.ToBool(value, defaultValue) ? "true" : "false";
}

//---------------------------
function DialogArguments()
{
    this.Opener = null;
    this.URL = "";
    this.Arguments = null;
}

//---------------------------
function ServerParam() { }

//---------------------------
function Page()
{
}
Page.FocusControl = function(id)
{
    try { document.getElementById(id).focus(); }
    catch (e) { }
}
Page.OpenWindow = function(url, width, height, name/*=null*/, features/*=null*/)
{
    if (width > screen.width) { width = screen.width - 50; }
    if (height > screen.height) { height = screen.height - 200; }

    var left = (screen.width - width) / 2;
    var top = (screen.height - height) / 2;

    features = DataType.ToString(features);
    if (features == "")
        features = "toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=yes,resizable=yes";
    features = features + ",height=" + height + ",width=" + width + ",left=" + left + ",top=" + top;

    var wnd = window.open(url, DataType.ToString(name, "windialog"), features);
    if (wnd != null)
        wnd.focus();
}
Page.ShowModal = function (url, width, height, bResizable/*=false*/, argument/*=null*/)
{
    if (typeof (argument) == 'undefined')
        argument = null;
    var args = new DialogArguments();
    args.Opener = window;
    args.URL = url;
    args.Arguments = argument;
    return window.showModalDialog("DlgContainer.htm", args,
				"dialogWidth:" + width + ";dialogHeight:" + height + ";status:0;resizable:" + (bResizable == true ? "yes;" : "no;"));
}
Page.ShowModeless = function(url, width, height, bResizable/*=false*/, argument/*=null*/)
{
    if (typeof (argument) == 'undefined')
        argument = null;
    var args = new DialogArguments();
    args.Opener = window;
    args.URL = url;
    args.Arguments = argument;
    return window.showModelessDialog("DlgContainer.htm", args,
				"dialogWidth:" + width + ";dialogHeight:" + height + ";status:0;resizable:" + (bResizable == true ? "yes;" : "no;"));
}
Page.OpenBrowserWindow = function(url, width, height)
{
    if (isIE())
    {
        if (isIE6OrLower())
            window.open(GetAppRootUrl() + "/" + url, "win", "width=" + width + "px,height=" + height + "px,toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=yes,resizable=yes");
        else
            Page.ShowModal(url, width + "px", height + "px", true);
    }
    else
        showModalDialog(url, false, "dialogWidth:" + width + "px;dialogHeight:" + height + "px;resizable=yes;");
}

Page.SetClientParam = function(name, value, submit)
{
    try
    {
        var hdnName = document.getElementById(ServerParam.ClientParamNameId);
        var hdnValue = document.getElementById(ServerParam.ClientParamValueId);

        hdnName.value = name;
        hdnValue.value = value;

        if (DataType.ToBool(submit, false))
            document.forms.item(0).submit();
    }
    catch (e)
    { }
}
Page.ConfirmServer = function(clientParamName, msg)
{
    if (confirm(msg))
        Page.SetClientParam(clientParamName, "OK", true);
    else
        Page.SetClientParam(clientParamName, "CANCEL", true);
}

function GetAppRootUrl()
{
    var appName = location.pathname.split("/");
    url = location.protocol + "//" + location.host + "/" + appName[1];
    return url;
}

function isIE()
{
    if (/MSIE[\/\s](\d+\.\d+)/.test(navigator.userAgent))
        return true;
    else
        return false;
}
function isIE6OrLower()
{
    if (/MSIE[\/\s](\d+\.\d+)/.test(navigator.userAgent))
    {
        var ieversion = new Number(RegExp.$1)
        if (ieversion <= 6)
            return true
        else
            return false;
    }
    else
        return false;

}


