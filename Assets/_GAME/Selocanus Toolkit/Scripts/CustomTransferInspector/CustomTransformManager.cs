namespace SelocanusToolkit
{
    public class CustomTransformManager
    {
        public bool TransformAdvancedUIEnabled = false;
        private static CustomTransformManager m_Singleton = null;

        public static CustomTransformManager Instance
        {
            get
            {
                if (m_Singleton == null)
                {
                    m_Singleton = new CustomTransformManager();
                }
                return m_Singleton;
            }
        }
    }
}

