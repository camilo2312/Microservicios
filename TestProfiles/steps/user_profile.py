from behave import *
import requests
import json
from faker import Faker
fake = Faker()

base_url = "http://localhost:4545/api/UserProfile"

#Escenario de creación de perfil
@given('soy un usuario nuevo')
def newUser(context):
    end_point = base_url + '/100'
    response = requests.get(end_point)
    context.response = response
    assert context.response.status_code == 404, 'Error al consultar cliente'

@when('se envia una solicitud para crear un perfil con datos aleatorios')
def create_profile(context):
    end_point = base_url
    payload = {
        "userId": "0",
        "personalPageUrl": "www.google.com",
        "nickname": fake.email(),
        "isContactInfoPublic": True,
        "mailingAddress": fake.email(),
        "biography": "Biografia " + fake.name(),
        "organization": "Universidad del quindio",
        "country": "Colombia",
        "socialLinks": [
            "facebook"
        ]
    }
    
    response = requests.post(end_point, json=payload)
    context.response = response

@then('se recibe una respuesta exitosa despues de crear el perfil')
def validate_response_create_profile(context):
    assert context.response.status_code == 200, 'Error al crear el perfil'

@given('usuario registrado previamente con ID {id}')
def validate_user(context, id):
    end_point = base_url + f'/{id}'
    response = requests.get(end_point)
    context.response = response
    assert context.response.status_code == 200, 'Error al consultar cliente'

@when('se envia una solicitud para obtener el perfil de un usuario con ID {id}')
def get_user(context, id):
    end_point = base_url + f'/{id}'
    response = requests.get(end_point)
    context.response = response    

@then('se recibe una respuesta exitosa con la informacion del perfil del usuario')
def validate_response_get_user(context):
    assert context.response.status_code == 200, 'Error al consultar el perfil'

@given('que exista un usuario con ID {id}')
def validate_profile(context, id):
    end_point = base_url + f'/{id}'
    response = requests.get(end_point)
    context.response = response
    assert context.response.status_code == 200, 'Error al consultar perfil'

@when('se envia una solicitud para actualizar la informacion del perfil con id {id}, con datos aleatorios')
def update_profile(context, id):
    end_point = base_url + f'/{id}'
    payload = {
        "userId": id,
        "personalPageUrl": "www.google.com",
        "nickname": fake.email(),
        "isContactInfoPublic": True,
        "mailingAddress": fake.email(),
        "biography": "Biografia " + fake.name(),
        "organization": "Universidad del quindio",
        "country": "Colombia",
        "socialLinks": [
            "facebook"
        ]
    }
    
    response = requests.put(end_point, json=payload)
    context.response = response

@then('se recibe una respuesta exitosa despues de actualizar el perfil')
def validate_response_update(context):
    assert context.response.status_code == 204, 'Error al actualizar el perfil'