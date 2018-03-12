using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Text.RegularExpressions;

namespace BulkEmailSMS
{
    public class clsSMS
    {

        #region Open and Close Ports
        //Open Port
        public SerialPort OpenPort(string p_strPortName, int p_uBaudRate, int p_uDataBits, int p_uReadTimeout, int p_uWriteTimeout)
        {
            receiveNow = new AutoResetEvent(false);
            SerialPort port = new SerialPort();

            try
            {
                port.PortName = p_strPortName;                 //COM1
                port.BaudRate = p_uBaudRate;                   //9600
                port.DataBits = p_uDataBits;                   //8
                port.StopBits = StopBits.One;                  //1
                port.Parity = Parity.None;                     //None
                port.ReadTimeout = p_uReadTimeout;             //300
                port.WriteTimeout = p_uWriteTimeout;           //300
                port.Encoding = Encoding.GetEncoding("iso-8859-1");
                port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                port.Open();
                port.DtrEnable = true;
                port.RtsEnable = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return port;
        }

        //Close Port
        public void ClosePort(SerialPort port)
        {
            try
            {
                port.Close();
                port.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                port = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        //Execute AT Command
        public string ExecCommand(SerialPort port, string command, int responseTimeout, string errorMessage)
        {
            try
            {
                port.DiscardOutBuffer();
                port.DiscardInBuffer();
                receiveNow.Reset();
                port.Write(command + "\r");

                string input = ReadResponse(port, responseTimeout);
                if ((input.Length == 0) || ((!input.EndsWith("\r\n> ")) && (!input.EndsWith("\r\nOK\r\n"))))
                    throw new ApplicationException("No success message was received.");
                return input;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Receive data from port
        public void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (e.EventType == SerialData.Chars)
                {
                    receiveNow.Set();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ReadResponse(SerialPort port, int timeout)
        {
            string buffer = string.Empty;
            try
            {
                do
                {
                    if (receiveNow.WaitOne(timeout, false))
                    {
                        string t = port.ReadExisting();
                        buffer += t;
                    }
                    else
                    {
                        if (buffer.Length > 0)
                            throw new ApplicationException("Response received is incomplete.");
                        else
                            throw new ApplicationException("No data received from phone.");
                    }
                }
                while (!buffer.EndsWith("\r\nOK\r\n") && !buffer.EndsWith("\r\n> ") && !buffer.EndsWith("\r\nERROR\r\n"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return buffer;
        }

     

        #region Read SMS

        public AutoResetEvent receiveNow;
        //public ShortMessageCollection ReadSMS(SerialPort port, string p_strCommand)
        //{

        //    // Set up the phone and read the messages
        //    ShortMessageCollection messages = null;
        //    try
        //    {

        //        #region Execute Command
        //        // Check connection
        //        ExecCommand(port, "AT", 300, "No phone connected");
        //        // Use message format "Text mode"
        //        ExecCommand(port, "AT+CMGF=1", 300, "Failed to set message format.");
        //        // Use character set "PCCP437"
        //        ExecCommand(port, "AT+CSCS=\"PCCP437\"", 300, "Failed to set character set.");
        //        // Select SIM storage
        //        ExecCommand(port, "AT+CPMS=\"SM\"", 300, "Failed to select message storage.");
        //        // Read the messages
        //        string input = ExecCommand(port, p_strCommand, 5000, "Failed to read the messages.");
        //        #endregion

        //        #region Parse messages
        //        messages = ParseMessages(input);
        //        #endregion

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    if (messages != null)
        //        return messages;
        //    else
        //        return null;

        //}
      
        //public ShortMessageCollection ParseMessages(string input)
        //{
        //    ShortMessageCollection messages = new ShortMessageCollection();
        //    try
        //    {
        //        Regex r = new Regex(@"\+CMGL: (\d+),""(.+)"",""(.+)"",(.*),""(.+)""\r\n(.+)\r\n");
        //        Match m = r.Match(input);
        //        while (m.Success)
        //        {
        //            ShortMessage msg = new ShortMessage();
        //            //msg.Index = int.Parse(m.Groups[1].Value);
        //            msg.Index = m.Groups[1].Value;
        //            msg.Status = m.Groups[2].Value;
        //            msg.Sender = m.Groups[3].Value;
        //            msg.Alphabet = m.Groups[4].Value;
        //            msg.Sent = m.Groups[5].Value;
        //            msg.Message = m.Groups[6].Value;
        //            messages.Add(msg);

        //            m = m.NextMatch();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return messages;
        //}

        #endregion

        #region Send SMS

        static AutoResetEvent readNow = new AutoResetEvent(false);

        public bool sendMsg(SerialPort port, string PhoneNo, string Message)
        {
            bool isSend = false;

            try
            {

                string recievedData = ExecCommand(port, "AT", 300, "No phone connected");
                recievedData = ExecCommand(port, "AT+CMGF=1", 300, "Failed to set message format.");
                String command = "AT+CMGS=\"" + PhoneNo + "\"";
                recievedData = ExecCommand(port, command, 300, "Failed to accept phoneNo");
                command = Message + char.ConvertFromUtf32(26) + "\r";
                recievedData = ExecCommand(port, command, 3000, "Failed to send message"); //3 seconds
                if (recievedData.EndsWith("\r\nOK\r\n"))
                {
                    isSend = true;
                }
                else if (recievedData.Contains("ERROR"))
                {
                    isSend = false;
                }
                return isSend;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //static void DataReceived(object sender, SerialDataReceivedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.EventType == SerialData.Chars)
        //            readNow.Set();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion

       
  

    }
}
