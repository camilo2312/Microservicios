Feature: Gestión de Perfiles
    Scenario: Crear Perfil
        Given: soy un usuario nuevo
        When se envia una solicitud para crear un perfil con datos aleatorios
        Then se recibe una respuesta exitosa despues de crear el perfil
    
    Scenario: Obtener perfil
        Give usuario registrado previamente con ID 1
        When se envia una solicitud para obtener el perfil de un usuario con ID 1
        Then se recibe una respuesta exitosa con la informacion del perfil del usuario
    
    Scenario: Actualizar informacion del perfil
        Given que exista un usuario con ID 1
        When se envia una solicitud para actualizar la informacion del perfil con id 1, con datos aleatorios
        Then se recibe una respuesta exitosa despues de actualizar el perfil