# Network Messaging Console Application

## Overview

This console application allows users to send and receive encrypted messages to and from devices on a network. The user interface can be navigated using the arrow keys, making it easy to send message requests, reply to messages, and manage encryption parameters. The application uses RSA encryption to ensure secure communication between devices.

## Features

- **User-Friendly Interface**: Navigate the application using the arrow keys.
- **Message Requests**: Send message requests to other devices on the network.
- **Encrypted Communication**: Replies to messages are encrypted using RSA and sent via TCP.
- **Decryption**: Received encrypted messages are decoded and displayed.
- **Encryption Management**: Users can change RSA encryption parameters to suit their needs.

## RSA Encryption

- The application utilizes RSA encryption to ensure message confidentiality and security.
- RSA encryption allows secure exchange of messages by encrypting replies before sending them via TCP.
