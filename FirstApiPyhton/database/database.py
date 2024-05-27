import mysql.connector

database = mysql.connector.connect(    
  host='mysql',
  port=3306,
  user='root',
  password='root',
  database='users'
  # host='localhost',
  # port=3309,
  # user='root',
  # password='root',
  # database='users'
)

def test_connection_mysql():
  try:
        # Configura la conexión a la base de datos MySQL
        connection = mysql.connector.connect(
            # host="localhost",
            # port=3309,
            # user="root",
            # password="erer",
            # database="users"
            host='mysql',
            port=3306,
            user='root',
            password='root',
            database='users'
        )

        if connection.is_connected():
            connection.close()  # Cierra la conexión después de verificarla
            return "READY"
        else:
            return "NO-READY"

  except mysql.connector.Error as e:
        print("Error de conexión a la base de datos MySQL: " + str(e))
        return "NO-READY"


