using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Media;
using RSend;

namespace Client
{
    public class ClientObject : MarshalByRefObject, IClientObject
    {
        public ClientForm clientForm = null;
        private delegate void txtMessagesUpdateDelegate(String senderName, String text);
        private Dictionary<string, string> _avatarColors = new Dictionary<string, string>();

        public ClientObject() { }
        public ClientObject(ClientForm form) { clientForm = form; }

        private string GetAvatarColor(string name)
        {
            if (string.IsNullOrEmpty(name)) return "#64B5F6";
            if (_avatarColors.ContainsKey(name)) return _avatarColors[name];

            int hash = name.GetHashCode();
            int r = (hash & 0xFF) % 200 + 55;
            int g = ((hash >> 8) & 0xFF) % 200 + 55;
            int b = ((hash >> 16) & 0xFF) % 200 + 55;
            string color = $"#{r:X2}{g:X2}{b:X2}";
            _avatarColors[name] = color;
            return color;
        }

        // Получение инициалов (первая буква или первые две)
        private string GetAvatar(string name)
        {
            if (string.IsNullOrEmpty(name)) return "?";

            // Убираем всё, что в скобках (компьютер, домен и т.п.)
            int bracketIndex = name.IndexOf('(');
            string cleanName = bracketIndex >= 0 ? name.Substring(0, bracketIndex).Trim() : name.Trim();

            if (string.IsNullOrEmpty(cleanName)) return "?";

            // Берём первые буквы слов (не более двух)
            string[] parts = cleanName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string initials = "";
            foreach (string p in parts)
            {
                if (p.Length > 0 && char.IsLetter(p[0]))
                    initials += p[0];
                if (initials.Length >= 2) break;
            }

            if (initials.Length == 0 && cleanName.Length > 0)
                initials = cleanName.Substring(0, 1);

            return initials.ToUpper();
        }

        public void SendMessageToClient(String senderNameInfo, String message, Boolean confirmation)
        {
            string originalMessage = message;
            String aliasJS = senderNameInfo.Split('(')[0].Trim();
            String senderName = senderNameInfo.Split(')')[0].Trim() + ")";
            String info = senderNameInfo.Split(')')[1].Trim();
            String senderNameDispaly = "";

            if ((info == null) || (info == ""))
                senderNameDispaly = senderName;
            else
                senderNameDispaly = info.Split(';')[0].Trim() + " (" + info.Split(';')[1].Trim() + ")";

            if (clientForm.hideHostName == "true")
                senderNameDispaly = senderNameDispaly.Split('(')[0].Trim();

            if (message == "selftest")
            {
                clientForm.selftestResult = true;
                return;
            }

            // Сохранение в историю (входящее)
            try
            {
                if (clientForm != null && !string.IsNullOrEmpty(originalMessage))
                {
                    string recipient = clientForm.userName ?? "Unknown";
                    HistoryManager.AddEntry(new HistoryEntry
                    {
                        Timestamp = DateTime.Now,
                        Sender = senderNameInfo,
                        Recipient = recipient,
                        Message = originalMessage,
                        IsIncoming = true
                    });
                }
            }
            catch { }

            // Преобразования для HTML (ссылки, экранирование)
            message = Regex.Replace(message, @"<", "&#60;");
            message = Regex.Replace(message, @">", "&#62;");
            message = Regex.Replace(message, @"\n", "<br>");
            message = Regex.Replace(message, @"  ", "&nbsp; ");

            message = Regex.Replace(message, @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)",
                "<span title='$1' style='COLOR: #0000ff; TEXT-DECORATION: underline; CURSOR: pointer;' onclick=\"window.external.openLink('$1')\">$1</span>",
                RegexOptions.IgnoreCase);

            message = Regex.Replace(message, @"(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}))",
                "<span title='mailto:$1' style='COLOR: #0000ff; TEXT-DECORATION: underline; CURSOR: pointer;' onclick=\"window.external.openLink('mailto:$1')\">$1</span>",
                RegexOptions.IgnoreCase);

            if (confirmation)
            {
                message = Regex.Replace(message, @"sDeliveryTotal", clientForm.SettingList["sDeliveryTotal"]);
                message = Regex.Replace(message, @"sDeliveryFailed", clientForm.SettingList["sDeliveryFailed"]);
            }

            // ===== АВАТАРКА С ЦЕНТРИРОВАНИЕМ (line-height работает в IE) =====
            string avatarColor = GetAvatarColor(senderNameDispaly);
            string avatar = GetAvatar(senderNameDispaly);
            string bubbleColor = "#E1F5FE";
            string textColor = "#000000";
            string time = DateTime.Now.ToString("HH:mm");
            string nameDisplay = senderNameDispaly;

            // Используем line-height = высоте блока для вертикального центрирования
            // и text-align: center для горизонтального
            string avatarStyle = "width:40px; height:40px; border-radius:50%; " +
                                 "background:" + avatarColor + "; " +
                                 "color:white; font-weight:bold; font-size:20px; " +
                                 "font-family:Arial; text-align:center; line-height:40px;";

            if (clientForm.modeReadOnly == "true")
            {
                string msgHtml = $@"
    <tr>
        <td style='padding: 6px 10px;'>
            <table cellpadding='0' cellspacing='0' style='max-width: 70%; background: {bubbleColor}; border-radius: 12px; padding: 8px 12px; box-shadow: 0 1px 2px rgba(0,0,0,0.15);'>
                <tr>
                    <td style='vertical-align: top; width: 46px; padding-right: 8px;'>
                        <div style='{avatarStyle}'>
                            {avatar}
                        </div>
                    </td>
                    <td>
                        <div style='font-weight: bold; color: #1565C0; font-size: 12px; font-family: Arial;'>
                            {nameDisplay}
                            <span style='color: #999; font-weight: normal; font-size: 10px; margin-left: 8px;'>{time}</span>
                        </div>
                        <div style='font-size: 14px; color: {textColor}; font-family: 'Segoe UI Emoji', 'Segoe UI', Arial; margin-top: 2px; word-wrap: break-word;'>
                            {message}
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr><td style='height: 4px;'></td></tr>";
                var msgText = msgHtml;
                txtMessagesUpdateDelegate WFUDelegate = new txtMessagesUpdateDelegate(SendMessageToClientReturned);
                clientForm.BeginInvoke(WFUDelegate, new object[] { senderName, msgText });
                return;
            }

            // Интерактивный режим (с кликом для ответа)
            string msgHtmlInteractive = $@"
    <tr>
        <td style='padding: 6px 10px;'>
            <table cellpadding='0' cellspacing='0' style='max-width: 70%; background: {bubbleColor}; border-radius: 12px; padding: 8px 12px; box-shadow: 0 1px 2px rgba(0,0,0,0.15);'>
                <tr>
                    <td style='vertical-align: top; width: 46px; padding-right: 8px;'>
                        <div style='{avatarStyle} cursor:pointer;' onclick=""window.external.getReplyName('{senderNameInfo}')"">
                            {avatar}
                        </div>
                    </td>
                    <td>
                        <div style='font-weight: bold; color: #1565C0; font-size: 12px; font-family: Arial; cursor: pointer;' onclick=""window.external.getReplyName('{senderNameInfo}')"">
                            {nameDisplay}
                            <span style='color: #999; font-weight: normal; font-size: 10px; margin-left: 8px;'>{time}</span>
                        </div>
                        <div style='font-size: 14px; color: {textColor}; font-family: 'Segoe UI Emoji', 'Segoe UI', Arial; margin-top: 2px; word-wrap: break-word;'>
                            {message}
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr><td style='height: 4px;'></td></tr>";

            txtMessagesUpdateDelegate WFUDelegate2 = new txtMessagesUpdateDelegate(SendMessageToClientReturned);
            clientForm.BeginInvoke(WFUDelegate2, new object[] { senderName, msgHtmlInteractive });
        }

        public void Test(String message)
        {
            MessageBox.Show(message, "client code");
        }

        public void SendMessageToClientReturned(String senderName, String text)
        {
            try
            {
                clientForm.sHTML = text + clientForm.sHTML;
                clientForm.webBrowser1.Document.OpenNew(true);
                clientForm.webBrowser1.Document.Write(clientForm.sHead + clientForm.sHTML + clientForm.sBottom);
                clientForm.Opacity = 1;
                clientForm.menuItemClear.Enabled = true;
                if (clientForm.Visible == false)
                {
                    clientForm.Visible = true;
                    clientForm.Activate();
                }
                NotifySound();
            }
            catch { }
        }

        public void NotifySound()
        {
            try
            {
                if (clientForm.notifySound.ToLower() == "true")
                {
                    SoundPlayer player = new SoundPlayer(Environment.GetEnvironmentVariable("windir") + "\\Media\\notify.wav");
                    player.Play();
                }
            }
            catch { }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Dictionary<String, String> SettingList = new Dictionary<String, String>();
            try
            {
                string GetValue(string key, string defaultValue = "")
                {
                    string val = ConfigurationSettings.AppSettings[key];
                    return (val != null) ? val.Trim() : defaultValue;
                }

                SettingList.Add("ipAddressServer", GetValue("ipAddressServer"));
                SettingList.Add("portServer", GetValue("portServer"));
                SettingList.Add("portClient", GetValue("portClient"));
                SettingList.Add("timeoutKeepAlive", GetValue("timeoutKeepAlive"));
                SettingList.Add("timeoutSelftest", GetValue("timeoutSelftest"));

                SettingList.Add("domainShowUserDisplay", GetValue("domainShowUserDisplay", "false").ToLower());
                SettingList.Add("domainShowHostDisplay", GetValue("domainShowHostDisplay", "false").ToLower());
                SettingList.Add("domainUserSearchAttribute", GetValue("domainUserSearchAttribute", ""));
                SettingList.Add("domainHostSearchAttribute", GetValue("domainHostSearchAttribute", ""));
                SettingList.Add("domainUserDisplayAttribute", GetValue("domainUserDisplayAttribute", ""));
                SettingList.Add("domainHostDisplayAttribute", GetValue("domainHostDisplayAttribute", ""));
                SettingList.Add("notifySound", GetValue("notifySound", "false").ToLower());
                SettingList.Add("modeReadOnly", GetValue("modeReadOnly", "false").ToLower());
                SettingList.Add("urlHelp", GetValue("urlHelp", ""));
                SettingList.Add("frmSendCaption", GetValue("frmSendCaption"));
                SettingList.Add("frmSelectCaption", GetValue("frmSelectCaption"));
                SettingList.Add("frmGroupCaption", GetValue("frmGroupCaption"));
                SettingList.Add("frmInfoCaption", GetValue("frmInfoCaption"));

                SettingList.Add("lblStatusOk", GetValue("lblStatusOk"));
                SettingList.Add("lblStatusErr", GetValue("lblStatusErr"));
                SettingList.Add("lblMsgText", GetValue("lblMsgText"));
                SettingList.Add("lblSendTo", GetValue("lblSendTo"));
                SettingList.Add("lblFilter", GetValue("lblFilter"));
                SettingList.Add("lblGroupName", GetValue("lblGroupName"));
                SettingList.Add("lblGroupMembers", GetValue("lblGroupMembers"));
                SettingList.Add("lblGroupComment", GetValue("lblGroupComment"));

                SettingList.Add("menuItemSendMsg", GetValue("menuItemSendMsg"));
                SettingList.Add("menuItemClear", GetValue("menuItemClear"));
                SettingList.Add("menuItemClose", GetValue("menuItemClose"));
                SettingList.Add("menuItemHelp", GetValue("menuItemHelp"));

                SettingList.Add("btnClear", GetValue("btnClear"));
                SettingList.Add("btnSelect", GetValue("btnSelect"));
                SettingList.Add("btnSend", GetValue("btnSend"));
                SettingList.Add("btnOk", GetValue("btnOk"));
                SettingList.Add("btnSave", GetValue("btnSave"));
                SettingList.Add("btnCancel", GetValue("btnCancel"));
                SettingList.Add("btnFilter", GetValue("btnFilter"));
                SettingList.Add("btnClose", GetValue("btnClose"));
                SettingList.Add("btnAddGroup", GetValue("btnAddGroup"));
                SettingList.Add("btnEditGroup", GetValue("btnEditGroup"));
                SettingList.Add("btnDelGroup", GetValue("btnDelGroup"));
                SettingList.Add("btnCopy", GetValue("btnCopy"));

                SettingList.Add("cbConfirm", GetValue("cbConfirm"));
                SettingList.Add("cbSelectAll", GetValue("cbSelectAll"));

                SettingList.Add("tabPageUser", GetValue("tabPageUser"));
                SettingList.Add("tabPageHost", GetValue("tabPageHost"));
                SettingList.Add("tabPagePrivateGroup", GetValue("tabPagePrivateGroup"));
                SettingList.Add("tabPageADGroup", GetValue("tabPageADGroup"));

                SettingList.Add("cbOptionStart", GetValue("cbOptionStart"));
                SettingList.Add("cbOptionExist", GetValue("cbOptionExist"));
                SettingList.Add("cbOptionNotStart", GetValue("cbOptionNotStart"));
                SettingList.Add("cbOptionNotExist", GetValue("cbOptionNotExist"));

                SettingList.Add("sToAllUsers", GetValue("sToAllUsers"));
                SettingList.Add("sMsgFrom", GetValue("sMsgFrom"));
                SettingList.Add("sDeliveryTotal", GetValue("sDeliveryTotal"));
                SettingList.Add("sDeliveryFailed", GetValue("sDeliveryFailed"));
                SettingList.Add("sTitleReply", GetValue("sTitleReply"));

                SettingList.Add("MsgBoxCaptionInfo", GetValue("MsgBoxCaptionInfo"));
                SettingList.Add("MsgBoxCaptionWarning", GetValue("MsgBoxCaptionWarning"));
                SettingList.Add("MsgBoxCaptionError", GetValue("MsgBoxCaptionError"));

                SettingList.Add("MsgBoxSendMsgOk", GetValue("MsgBoxSendMsgOk"));
                SettingList.Add("MsgBoxTwoAppRun", GetValue("MsgBoxTwoAppRun"));
                SettingList.Add("MsgBoxExit", GetValue("MsgBoxExit"));
                SettingList.Add("MsgBoxClear", GetValue("MsgBoxClear"));
                SettingList.Add("MsgBoxNoReplyMsg", GetValue("MsgBoxNoReplyMsg"));
                SettingList.Add("MsgBoxNoReplySelect", GetValue("MsgBoxNoReplySelect"));
                SettingList.Add("MsgBoxNoRegisteredUsers", GetValue("MsgBoxNoRegisteredUsers"));
                SettingList.Add("MsgBoxNoReplyAlert", GetValue("MsgBoxNoReplyAlert"));
                SettingList.Add("MsgBoxNoInfoAlert", GetValue("MsgBoxNoInfoAlert"));
                SettingList.Add("MsgBoxEmptyMsgTextBox", GetValue("MsgBoxEmptyMsgTextBox"));
                SettingList.Add("MsgBoxEmptySelectUserTextBox", GetValue("MsgBoxEmptySelectUserTextBox"));
                SettingList.Add("MsgBoxEmptyField", GetValue("MsgBoxEmptyField"));
                SettingList.Add("MsgBoxOneGroupEdit", GetValue("MsgBoxOneGroupEdit"));
                SettingList.Add("MsgBoxEmptyGroupEdit", GetValue("MsgBoxEmptyGroupEdit"));
                SettingList.Add("MsgBoxEmptyGroupDelete", GetValue("MsgBoxEmptyGroupDelete"));
                SettingList.Add("MsgBoxConfirmDeleteGroup", GetValue("MsgBoxConfirmDeleteGroup"));
                SettingList.Add("MsgBoxNoPing", GetValue("MsgBoxNoPing"));
                SettingList.Add("MsgBoxNoRemoting", GetValue("MsgBoxNoRemoting"));
                SettingList.Add("MsgBoxErrSendMsg", GetValue("MsgBoxErrSendMsg"));
                SettingList.Add("MsgBoxErrListSendTo", GetValue("MsgBoxErrListSendTo"));
                SettingList.Add("MsgBoxErrSaveGroup", GetValue("MsgBoxErrSaveGroup"));
                SettingList.Add("MsgBoxErrDeleteGroup", GetValue("MsgBoxErrDeleteGroup"));
                SettingList.Add("MsgBoxErrOpenHelp", GetValue("MsgBoxErrOpenHelp"));

                try { SettingList.Add("hideHostName", GetValue("hideHostName", "false").ToLower()); }
                catch { SettingList.Add("hideHostName", "false"); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Configuration file not found or damaged!\n" + ex.Message, "Error", MessageBoxButtons.OK);
                Application.Exit();
                return;
            }

            String proc = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(proc);
            if (processes.Length > 1)
            {
                MessageBox.Show(SettingList["MsgBoxTwoAppRun"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);
                Application.Exit();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ClientForm clientFormHide = new ClientForm(SettingList);
            clientFormHide.Opacity = 0;
            clientFormHide.Visible = true;
            Application.Run();
        }
    }
}