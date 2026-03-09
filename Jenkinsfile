
pipeline {
    agent any

    stages {

        stage('Checkout') {
            steps {
                git 'https://github.com/Hayk-01/qr_bonus.git'
            }
        }

        stage('Build') {
            steps {
                sh 'pwd'
                sh 'cd /var/lib/jenkins/workspace/QR_Bonus_Api/QRBonus.API/QRBonus.Admin'
                sh 'dotnet build -c Release /p:Version=${BUILD_NUMBER}'
                sh 'dotnet publish -c Release --no-build'
            }
        }

        stage('Test') {
            steps {
                sh 'npm test'
            }
        }

        stage('Deploy') {
            steps {
                sh './deploy.sh'
            }
        }

    }
}
