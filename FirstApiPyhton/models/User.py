class User:
    cedula = ''
    nombre = ''
    apellido = ''
    email = ''
    contrasena = ''
    
    def __init__(self, cedula, nombre, apellido, email, contrasena):
        self.cedula = cedula
        self.nombre = nombre
        self.apellido = apellido
        self.email = email
        self.contrasena = contrasena