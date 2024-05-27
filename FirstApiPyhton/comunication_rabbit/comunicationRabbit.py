import pika

# Conectarse al servidor RabbitMQ
connection = pika.BlockingConnection(pika.ConnectionParameters('rabbitmq', '5672'))
channel = connection.channel()

# Cola para enviar mensajes
channel.queue_declare(queue='autenticaciones')
channel.queue_declare(queue='registros')

def enviar_mensaje_usuario_autenticado(usuario):
    # Enviar un mensaje indicando que el usuario se autenticó
    print(f"Mensaje de autenticaciòn")
    mensaje = f"{usuario}"
    channel.basic_publish(exchange='', routing_key='autenticaciones', body=mensaje)
    print(f"Mensaje enviado: {mensaje}")
    
def enviar_mensaje_usuario_registrado(usuario):
    # Enviar un mensaje indicando que el usuario se autenticó
    print(f"Mensaje de registro")
    mensaje = f"{usuario}"
    channel.basic_publish(exchange='', routing_key='registros', body=mensaje)
    print(f"Mensaje enviado: {mensaje}")

# Cerrar la conexión
# connection.close()
