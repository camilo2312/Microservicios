FROM python:3.10-alpine

RUN apk update \
    && apk add --no-cache libffi libffi-dev build-base python3-dev \
    && pip3 install --upgrade pip

# Configura el entorno
ENV CFFI_VERSION=1.14.5

RUN pip3 install cffi==$CFFI_VERSION cryptography

WORKDIR /app

COPY . /app

RUN pip3 --no-cache-dir install -r requirements.txt

CMD ["python3", "main.py"]