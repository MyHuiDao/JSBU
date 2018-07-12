using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CClient
{
    /// <summary>
    /// 发送逻辑：两个Send，先发送总长度，再发送信息，信息包括协议头和信息
    /// </summary>
    class Message
    {
        byte[] bytes;
        int writePos = 0;//写入的位置
        int readPos = 0;//读取的位置
        public static Message New()
        {
            Message message = new Message();//创建个Messege对象，接着创建了个byte数组，用来存放传递的信息
            return message;
        }
        private Message()
        {
            bytes = new byte[1024 * 1000];
        }
        public void writeInt(int value)//10
        {
            byte[] b = BitConverter.GetBytes(value);
            this.writeBytes(b, b.Length);
        }
        public void writeString(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            writeInt(b.Length);//先把长度写进去
            writeBytes(b, b.Length);
        }
        private void writeBytes(byte[] bs, int len)
        {
            for (int i = 0; i < len; i++)
            {
                bytes[writePos] = bs[i];
                writePos++;
            }
        }


        public int readInt()
        {

            int intNum = BitConverter.ToInt32(bytes, readPos);//前四个字节转换
            readPos += 4;
            return intNum;


        }
        /// <summary>
        /// 先读取长度，再一个字节一个字节读取
        /// </summary>
        /// <returns></returns>
        public string readString() {

            int count = readInt();//读取字符串的长度
            string s = Encoding.UTF8.GetString(bytes, readPos, count);
            return s;

        }

        public void Clear()
        {

            Array.Clear(bytes, 0, bytes.Length);
            writePos = 0;
            readPos = 0;
        }


        public byte[] Bytes
        {
            get { return bytes; }
        }
        public int Count
        {
            get { return writePos; }
        }
        public byte[] CountBytes
        {
            get {
                return BitConverter.GetBytes(writePos);
            }
        }
    }

}
