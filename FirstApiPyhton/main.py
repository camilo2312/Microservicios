from flask import Flask, jsonify, request, redirect
from flask_swagger_ui import get_swaggerui_blueprint
from flask_cors import CORS
import time

from models.User import User
from security.Security import Security
from services.user_service import UserService
from database.database import test_connection_mysql
import comunication_rabbit.comunicationRabbit as rabbit
import json

app = Flask(__name__)
CORS(app)

usuarios_version = "1.0"
usuarios_tiempo_inicio = time.time()

SWAGGER_URL = '/api/docs'  # URL for exposing Swagger UI (without trailing '/')
API_URL = '/static/swagger.json'  # Our API url (can of course be a local resource)

# Call factory function to create our blueprint
swaggerui_blueprint = get_swaggerui_blueprint(
    SWAGGER_URL,  # Swagger UI static files will be mapped to '{SWAGGER_URL}/dist/'
    API_URL,
    config={  # Swagger UI config overrides
        'app_name': "First api Python"
    },
    # oauth_config={  # OAuth config. See https://github.com/swagger-api/swagger-ui#oauth2-configuration .
    #    'clientId': "your-client-id",
    #    'clientSecret': "your-client-secret-if-required",
    #    'realm': "your-realms",
    #    'appName': "your-app-name",
    #    'scopeSeparator': " ",
    #    'additionalQueryStringParams': {'test': "hello"}
    # }
)

app.register_blueprint(swaggerui_blueprint)

@app.route('/')
def home():
    return redirect('/api/docs')

@app.route('/login', methods=['POST'])
def login():
    email = request.json['email']
    password = request.json['password']

    if email and password:
        result = UserService.login(email, password)
        if (len(result) > 0):
            json = '{"Application": "Aplicación de usuarios", "LogType": "INFO", "Module": "Modulo de autenticaciones", "Timestamp": "2024-04-16", "Summary": "Descripcion", "Description": "Usuario ' + email + '"}'                                  
            token = Security.generate_token(email)
            print('voy a enviar el mensaje a rabbit ' + json)
            rabbit.enviar_mensaje_usuario_autenticado(json)
            return jsonify({'status': 200, 'message': 'Token generado', 'token': token}), 200
        else:
            return jsonify({'status': 400, 'message': 'Usuario y/o contraseña invalidos'}), 400
    else:
        return jsonify({'status': 400, 'message': 'Usuario y contraseña obligatorios'}), 400

@app.route('/users', methods=['GET'])
def get_users():
    
    payload = Security.validate_token(request.headers)
    page_number = int(request.args.get('page_number'));
    limit = int(request.args.get('limit'));
    
    if payload:
        result = UserService.get_users(page_number, limit)
        if (len(result.users) > 0):
            return jsonify({'status': 200, 'total': result.total, 'data': result.users}), 200
        else:
            return jsonify({'status': 400, 'message': 'No existen usuarios registrados'}), 400
    else:
        return jsonify({'status': 401, 'message': 'No esta autorizado para realizar esta acción'}), 401
    

@app.route('/user/<string:id>', methods=['GET'])
def get_user_id(id: str):
    # payload = Security.validate_token(request.headers)
    # if payload:
        result = UserService.get_user_id(id)
        if (len(result) > 0):
            return jsonify({'status': 200, 'data': result}), 200
        else:
            return jsonify({'status': 400, 'message': 'No existe usuario con cedula ' + id}), 400
    # else:
    #     return jsonify({'status': 401, 'message': 'No esta autorizado para realizar esta acción'}), 401
    
@app.route('/users/password/<string:id>', methods=['GET'])
def get_password(id: str):
    payload = Security.validate_token(request.headers)
    if payload:
        result = UserService.get_password(id)
        if (len(result) > 0):
            token = Security.generate_token_password(id)
            return jsonify({'status': 200, 'url': 'http://localhost:6061/users/password', 'token': token}), 200
        else:
            return jsonify({'status': 400, 'message': 'No existe registro de contraseña de usuario con cedula ' + id}), 400
    else:
        return jsonify({'status': 401, 'message': 'No esta autorizado para realizar esta acción'}), 401
        

@app.route('/users', methods=['POST'])
def register_user():
    
    user = User(request.json['cedula'], request.json['nombre'], request.json['apellido'], request.json['email'], request.json['contrasena'])
    result = UserService.register_user(user)
    if (result) : 
        data = { 
            "UserId": 0,
            "PersonalPageUrl": f"Pagina de {user.nombre}",
            "Nickname": f"{user.email}",
            "IsContactInfoPublic": True,
            "MailingAddress":  f"{user.email}",
            "Biography": f"Biografia de {user.nombre}",
            "Organization": f"Organización de {user.nombre}",
            "Country": "Colombia",
            "Id": user.cedula,
            "SocialLinks": ["www.google.com"]
        }
        rabbit.enviar_mensaje_usuario_registrado(json.dumps(data))
        return jsonify({'status': 200, 'message': 'Usuario registrado correctamente'}), 200
    else:
        return jsonify({'status': 400, 'message': 'No fue posible registrar el usuario'}), 400
    
@app.route('/users/<string:id>', methods=['PUT'])
def update_user(id: str):
    
    payload = Security.validate_token(request.headers)
    
    if payload:
        user = User(request.json['cedula'], request.json['nombre'], request.json['apellido'], request.json['email'], request.json['contrasena'])
        result = UserService.update_user(id, user)
        if (result) : 
            return jsonify({'status': 200, 'message': 'Usuario actualizado correctamente'}), 200
        else:
            return jsonify({'status': 400, 'message': 'No fue posible actualizar el usuario'}), 400
    else:
        return jsonify({'status': 401, 'message': 'No esta autorizado para realizar esta acción'}), 401
    
@app.route('/users/password', methods=['PATCH'])
def update_password():
    
    payload = Security.validate_token(request.headers)
    
    if payload:
        new_password = request.json['new_password']
        print(payload)
        result = UserService.update_password(payload['cedula'], new_password)
        if (result) : 
            return jsonify({'status': 200, 'message': 'Contraseña actualizada correctamente'}), 200
        else:
            return jsonify({'status': 400, 'message': 'No fue posible actualizar la contraseña'}), 400
    else:
        return jsonify({'status': 401, 'message': 'No esta autorizado para realizar esta acción'}), 401
    
@app.route('/users/<string:id>', methods=['DELETE'])
def delete_user(id: str):
    
    payload = Security.validate_token(request.headers)
    
    if payload:
        result = UserService.delete_user(id)
        if (result) : 
            return jsonify({'status': 200, 'message': 'Usuario eliminado correctamente'}), 200
        else:
            return jsonify({'status': 400, 'message': 'No fue posible eliminar el usuario no existe'}), 400
    else:
        return jsonify({'status': 401, 'message': 'No esta autorizado para realizar esta acción'}), 401
    

# Sección para obtener la salud de la api
# Método encargado de obtener el tiempo
def obtener_tiempo_ejecucion(tiempo_inicio):
    tiempo_actual = time.time()
    tiempo_transcurrido = tiempo_actual - tiempo_inicio
    return tiempo_transcurrido

@app.route('/health')
def health():
    
    return jsonify({
        "status": "UP",
        "checks": [
            {
                "status": test_connection_mysql(),
                "version": "1.0",
                "uptime": obtener_tiempo_ejecucion(usuarios_tiempo_inicio)
            },
            {
                "status": "live",
                "version": "1.0",
                "uptime": obtener_tiempo_ejecucion(usuarios_tiempo_inicio)
            }
        ]
    })

@app.route("/health/ready")
def ready():
    return jsonify({
        "status": test_connection_mysql(),
        "version": usuarios_version,
        "uptime": obtener_tiempo_ejecucion(usuarios_tiempo_inicio)
    })

@app.route("/health/live")
def live():
    return jsonify({
        "status": "live",
        "version": usuarios_version,
        "uptime": obtener_tiempo_ejecucion(usuarios_tiempo_inicio)
    })

if __name__ == "__main__":
    app.run(host='0.0.0.0', port=9091, debug=True)
