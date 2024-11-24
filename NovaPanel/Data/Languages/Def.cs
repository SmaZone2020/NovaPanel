namespace N0v4P4n31.Data.Languages
{
    public class Def
    {
        public string Get(string variableName)
        {
            switch (variableName)
            {
                case "Dashboard":
                    return Dashboard;
                case "Welcome":
                    return Welcome;
                case "ToolBox":
                    return ToolBox;
                case "DomainName":
                    return DomainName;
                case "RestartServer":
                    return RestartServer;
                case "User":
                    return User;
                case "Password":
                    return Password;
                case "Port":
                    return Port;
                case "IpAddress":
                    return IpAddress;
                case "WhiteList":
                    return WhiteList;
                case "BlackList":
                    return BlackList;
                case "TwoFA":
                    return TwoFA;
                case "Device":
                    return Device;
                case "File":
                    return File;
                case "Server":
                    return Server;
                case "Pay":
                    return Pay;
                case "Data":
                    return Data;
                case "Date":
                    return Date;
                case "About":
                    return About;
                case "Plugins":
                    return Plugins;
                case "Store":
                    return Store;
                case "App":
                    return App;
                case "Project":
                    return Project;
                case "Subject":
                    return Subject;
                case "WebSite":
                    return WebSite;
                case "State":
                    return State;
                case "Overview":
                    return Overview;
                case "System":
                    return System;
                case "RAM":
                    return RAM;
                case "Version":
                    return Version;
                case "SystemBit":
                    return SystemBit;
                case "StartUpTime":
                    return StartUpTime;
                case "RunningTime":
                    return RunningTime;
                case "DataBase":
                    return DataBase;
                case "NetworkMonitoring":
                    return NetworkMonitoring;
                case "UpLink":
                    return UpLink;
                case "DownLink":
                    return DownLink;
                case "TotalReceived":
                    return TotalReceived;
                case "TotalSends":
                    return TotalSends;
                case "SecureEntrance":
                    return SecureEntrance;
                case "Docker":
                    return Docker;
                case "Shell":
                    return Shell;
                case "Setting":
                    return Setting;
                default:
                    return "";
            }
        }
        public string Dashboard { get; } = "仪表盘";
        public string Welcome { get; } = "欢迎";
        public string ToolBox { get; } = "工具箱";
        public string DomainName { get; } = "域名";
        public string RestartServer { get; } = "重启服务器";
        public string User { get; } = "用户";
        public string Password { get; } = "密码";
        public string Port { get; } = "端口";
        public string IpAddress { get; } = "Ip地址";
        public string WhiteList { get; } = "白名单";
        public string BlackList { get; } = "黑名单";
        public string Disk { get; } = "磁盘";
        public string TwoFA { get; } = "两部验证";
        public string Device { get; } = "设备";
        public string File { get; } = "文件";
        public string Server { get; } = "服务器";
        public string Pay { get; } = "支付";
        public string Data { get; } = "数据";
        public string Date { get; } = "日期";
        public string About { get; } = "关于";
        public string Plugins { get; } = "插件";
        public string Store { get; } = "商城";
        public string App { get; } = "应用";
        public string Project { get; } = "项目";
        public string Subject { get; } = "主题";
        public string WebSite { get; } = "网站";
        public string State { get; } = "状态";
        public string Overview { get; } = "概览";
        public string System { get; } = "系统";
        public string RAM { get; } = "内存";
        public string Version { get; } = "版本";
        public string SystemBit { get; } = "系统类型";
        public string StartUpTime { get; } = "启动时间";
        public string RunningTime { get; } = "运行时间";
        public string DataBase { get; } = "数据库";
        public string NetworkMonitoring { get; } = "网络监控";
        public string UpLink { get; } = "上行";
        public string DownLink { get; } = "下行";
        public string TotalReceived { get; } = "总接收";
        public string TotalSends { get; } = "总发送";
        public string SecureEntrance { get; } = "安全入口";
        public string Docker { get; } = "容器";
        public string Shell { get; } = "终端";
        public string Setting { get; } = "设置";

    }
}
