namespace TableData
{
    [Serializable]
    public class HashTableData
    {
        int funtion = 0;
        string key = "";
        object value = null;

        public HashTableData(int funtion, string key, object value)
        {
            this.funtion = funtion;
            this.key = key;
            this.value = value;
        }
        public int getFuntion()
        {
            return funtion;
        }
        public string getKey()
        {
            return key;
        }
        public object getvalue()
        {
            return value;
        }
        public void setvalue(Object v)
        {
            this.value = v;
        }
    }
}