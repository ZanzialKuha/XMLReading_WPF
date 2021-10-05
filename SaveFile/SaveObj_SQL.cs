using System.Data;
using System.Data.SqlClient;

namespace XMLReading_WPF
{
    class SaveObj_SQL : SaveObj
    {
        private SqlConnection connection { get; set; }
        private string XML { get; set; }
        public SaveObj_SQL(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            connection.Open();

            if (connection.State == ConnectionState.Open)
                IsReady = true;
            else 
                IsReady = false;
        }
        public override void Add(string text)
        {
            XML += text;
        }
        public override void Save()
        {
            SqlCommand command = new SqlCommand();

            XML = "<Houses>" + XML + "</Houses>";

            command.CommandText = " DECLARE @xml xml = '" + XML + "'"
                                 + " INSERT INTO Houses"
                                 + " SELECT T.c.value('./@POSTALCODE', 'VARCHAR(256)') 'Почтовый индекс'"
                                      + " , T.c.value('./@REGIONCODE', 'VARCHAR(256)') 'Код региона'"
                                      + " , T.c.value('./@IFNSFL', 'VARCHAR(256)') 'Код ИФНС ФЛ'"
                                      + " , T.c.value('./@TERRIFNSFL', 'VARCHAR(256)') 'Код территориального участка ИФНС ФЛ'"
                                      + " , T.c.value('./@IFNSUL', 'VARCHAR(256)') 'Код ИФНС ЮЛ'"
                                      + " , T.c.value('./@TERRIFNSUL', 'VARCHAR(256)') 'Код территориального участка ИФНС ЮЛ'"
                                      + " , T.c.value('./@OKATO', 'VARCHAR(256)') 'OKATO'"
                                      + " , T.c.value('./@OKTMO', 'VARCHAR(256)') 'OKTMO'"
                                      + " , T.c.value('./@UPDATEDATE', 'VARCHAR(256)') 'Дата время внесения записи'"
                                      + " , T.c.value('./@HOUSENUM', 'VARCHAR(256)') 'Номер дома'"
                                      + " , T.c.value('./@ESTSTATUS', 'VARCHAR(256)') 'Признак владения'"
                                      + " , T.c.value('./@BUILDNUM', 'VARCHAR(256)') 'Номер корпуса'"
                                      + " , T.c.value('./@STRUCNUM', 'VARCHAR(256)') 'Номер строения'"
                                      + " , T.c.value('./@STRSTATUS', 'VARCHAR(256)') 'Признак строения'"
                                      + " , T.c.value('./@HOUSEID', 'VARCHAR(256)') 'Уникальный идентификатор записи дома'"
                                      + " , T.c.value('./@HOUSEGUID', 'VARCHAR(256)') 'Глобальный уникальный идентификатор дома'"
                                      + " , T.c.value('./@AOGUID', 'VARCHAR(256)') 'Guid записи родительского объекта (улицы, города, населенного пункта и т.п.)'"
                                      + " , T.c.value('./@STARTDATE', 'VARCHAR(256)') 'Начало действия записи'"
                                      + " , T.c.value('./@ENDDATE', 'VARCHAR(256)') 'Окончание действия записи'"
                                      + " , T.c.value('./@STATSTATUS', 'VARCHAR(30)') 'Состояние дом'"
                                    + " FROM @xml.nodes('Houses/House') T(c)";
            command.Connection = connection;
            command.ExecuteNonQuery();
            XML = "";
        }
        public override void Finish()
        {
            connection.Close();
        }
    }
}
