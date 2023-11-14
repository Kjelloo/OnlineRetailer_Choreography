pipeline {
    agent any
    triggers {
        pollSCM("* * * * *")
    }
    stages {
        stage("Build") {
            steps {
                bat 'docker compose build'
            }
        }
        stage("Delivery") {
            steps {
                withCredentials([usernamePassword(credentialsId: 'dockerhub', usernameVariable: 'USERNAME', passwordVariable: 'PASSWORD')]) {
                    bat 'docker login -u %USERNAME% -p %PASSWORD%'
                    bat 'docker compose push'
                }
            }
        }
        stage("Deployment") {
            steps {
                bat 'docker compose up -d'
            }
        }
    }
}
