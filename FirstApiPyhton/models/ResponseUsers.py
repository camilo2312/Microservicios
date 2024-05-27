
class ResponseUsers:
    users = []
    total = 0
    
    def __init__(self, users: [], total: int): # type: ignore
        self.users = users
        self.total = total