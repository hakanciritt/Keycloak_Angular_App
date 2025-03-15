steps for running project

1 - go to terminal and running docker command line 


docker run -p 8080:8080 -e KC_BOOTSTRAP_ADMIN_USERNAME=admin -e KC_BOOTSTRAP_ADMIN_PASSWORD=admin quay.io/keycloak/keycloak:26.1.3 start-dev

![Screenshot 2025-03-11 at 23 13 43](https://github.com/user-attachments/assets/22aa4635-fd18-41de-a9a1-8823a6401729)


2 - after doing of appsettings.json file configuration, web api project start.

![Screenshot 2025-03-11 at 23 09 33](https://github.com/user-attachments/assets/39c01e46-5318-4837-8b6e-0c15b57dbff9)

3 - run angular project above command lines


npm install

ng serve --open

![Screenshot 2025-03-15 at 14 12 53](https://github.com/user-attachments/assets/0f19a7cf-4c71-46f7-8b38-8d743f872615)

![Screenshot 2025-03-15 at 14 13 16](https://github.com/user-attachments/assets/918ef552-c27b-4a9a-98f4-1a47cf9edb21)

![Screenshot 2025-03-15 at 14 13 31](https://github.com/user-attachments/assets/8a604fc7-39e0-484e-9e20-3d21ef4d49be)


