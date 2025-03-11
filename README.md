for running project. steps

1 - go to terminal and running docker command line 


docker run -p 8080:8080 -e KC_BOOTSTRAP_ADMIN_USERNAME=admin -e KC_BOOTSTRAP_ADMIN_PASSWORD=admin quay.io/keycloak/keycloak:26.1.3 start-dev

2 - after doing of appsettings.json file configuration, web api project start.


![Screenshot 2025-03-11 at 23 09 33](https://github.com/user-attachments/assets/39c01e46-5318-4837-8b6e-0c15b57dbff9)

3 - run angular project above command lines


npm install

ng serve --open

![Screenshot 2025-03-11 at 23 10 30](https://github.com/user-attachments/assets/8cf72630-8a11-471c-97ab-893abc3c65c5)

![Screenshot 2025-03-11 at 23 11 00](https://github.com/user-attachments/assets/98a24483-5b4b-4c8d-be61-10be9ac3023a)
