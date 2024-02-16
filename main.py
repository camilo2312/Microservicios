from flask import Flask, jsonify, request

from models.User import User
from security.Security import Security

app = Flask(__name__)

@app.route('/saludo2', methods=['GET'])
def saludo2():
    name = request.args.get('nombre')
    
    if name:
        return jsonify({'status': 200, 'message': f'Hola {name}'})
    else:
        return jsonify({'status': 400, 'message': 'El nombre es requerido'}), 400

@app.route('/saludo', methods=['GET'])
def saludo():
    name = request.args.get('nombre')
    payload = Security.validate_token(request.headers)
    
    if payload:
        if payload['username'] == name:
            return jsonify({'status': 200, 'message': f'Hola {name}'})
        else:
            return jsonify({'status': 400, 'message': 'Los nombres no corresponden'}), 400
    else:
        return jsonify({'status': 401, 'message': 'No esta autorizado'}), 401

@app.route('/login', methods=['POST'])
def login():
    username = request.json['username']
    password = request.json['password']

    if username and password:
        user = User(username, password)
        token = Security.generate_token(user)
        return jsonify({'status': 200, 'message': 'Token generado', 'token': token}), 200
    else:
        return jsonify({'status': 400, 'message': 'Usuario y contraseña obligatorios'}), 400

if __name__ == "__main__":
    app.run(host='0.0.0.0', port=80, debug=True)
