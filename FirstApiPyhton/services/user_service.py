from models.ResponseUsers import ResponseUsers
from models.User import User
from database.database import database as db

class UserService:


    @classmethod
    def register_user(self, user: User):
        cursor = db.cursor()
        sql = 'INSERT INTO Users (cedula, nombre, apellido, email, contrasena) VALUES (%s, %s, %s, %s, %s)'
        data = (user.cedula, user.nombre, user.apellido, user.email, user.contrasena)
        cursor.execute(sql, data)
        db.commit()
        return user

    @classmethod
    def update_user(self, id, user: User):
        cursor = db.cursor()
        sql = 'UPDATE Users SET nombre = %s, apellido = %s, email = %s WHERE cedula = %s'
        data = (user.nombre, user.apellido, user.email, id)
        cursor.execute(sql, data)
        db.commit()
        return True
    
    @classmethod
    def     update_password(self, id, new_password: str):
        cursor = db.cursor()
        sql = 'UPDATE Users SET contrasena = %s WHERE cedula = %s'
        data = (new_password, id)
        cursor.execute(sql, data)
        db.commit()
        return True
    
    @classmethod
    def delete_user(self, id):
        cursor = db.cursor()
        sql = 'DELETE FROM Users WHERE cedula = ' + f"'{id}'"        
        cursor.execute(sql)
        db.commit()
        return True
    
    @classmethod
    def get_users(self, page_number: int, limit: int):
        cursor = db.cursor()
        sql = 'SELECT * FROM Users LIMIT ' + str(limit) + ' OFFSET ' + str(((page_number - 1) * limit))
        cursor.execute(sql)
        
        users = [dict((cursor.description[i][0], value) \
               for i, value in enumerate(row)) for row in cursor.fetchall()]
        
        sql = 'SELECT count(*) FROM Users'
        cursor.execute(sql)
        total = cursor.fetchone()
        
        response = ResponseUsers(users, total[0])
        return response
        
    @classmethod
    def get_user_id(self, id):
        cursor = db.cursor()
        sql = 'SELECT * FROM Users WHERE cedula = ' + f"'{id}'"
        cursor.execute(sql)
        
        users = [dict((cursor.description[i][0], value) \
               for i, value in enumerate(row)) for row in cursor.fetchall()]
        return users
    
    @classmethod
    def get_password(self, id):
        cursor = db.cursor()
        sql = 'SELECT cedula FROM Users WHERE cedula = ' + f"'{id}'"
        cursor.execute(sql)
        
        users = [dict((cursor.description[i][0], value) \
               for i, value in enumerate(row)) for row in cursor.fetchall()]
        return users
    
    @classmethod
    def login(self, email: str, password: str):
        cursor = db.cursor()
        sql = f"SELECT * FROM Users WHERE email = '{email}' AND contrasena = '{password}'"
        cursor.execute(sql)
        
        users = [dict((cursor.description[i][0], value) \
               for i, value in enumerate(row)) for row in cursor.fetchall()]
        return users