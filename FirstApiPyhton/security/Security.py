import datetime
import pytz
import jwt


class Security:

    tz = pytz.timezone("America/Bogota")
    secret_key = 'Sharp'

    @classmethod
    def generate_token(cls, email: str):
        date = datetime.datetime.now(tz = cls.tz)
        payload = {
            'iat': date,
            'exp': date + datetime.timedelta(hours=1),
            'email': email,
            'iss': 'ingesis.uniquindio.edu.co'
        }

        return jwt.encode(payload, cls.secret_key, algorithm='HS256')
    
    @classmethod
    def generate_token_password(cls, cedula: str):
        date = datetime.datetime.now(tz = cls.tz)
        payload = {
            'iat': date,
            'exp': date + datetime.timedelta(hours=1),
            'cedula': cedula,
            'iss': 'ingesis.uniquindio.edu.co'
        }

        return jwt.encode(payload, cls.secret_key, algorithm='HS256')
    
    @classmethod
    def validate_token(cls, headers):
        if 'Authorization' in headers.keys():
            authorization = headers['Authorization']
            encoded_token = authorization.split(' ')[1]
            try:
                payload = jwt.decode(encoded_token, cls.secret_key, algorithms=['HS256'])
                return payload
            except (jwt.ExpiredSignatureError, jwt.InvalidSignatureError):
                return ""
        else:
            return ""