pipeline {
  agent any
  tools { nodejs "node" }
  stages {
    stage('Build API') {
      steps {
        dir("api") {
          sh 'dotnet clean -c Release'
          sh 'dotnet build -c Release'
        }
      }
    }
    stage('Build UI') {
      steps {
        dir("ui") {
          sh 'npm ci'
          sh 'npm run build -- --prod'
        }
      }
    }
    // make sure `JOURNALLY_CONN_STR` env. variable is set.
    stage('Deploy DB Migrations') {
      steps {
        dir("api") {
          sh 'dotnet ef database update'
        }
      }
    }
    stage('Deploy API') {
      when { branch 'master' }
      steps {
        dir("api") {
          azureFunctionAppPublish appName: "journally",
          azureCredentialsId: 'jenkins-sp',
          resourceGroup: "journally-prod-rg",
          sourceDirectory: 'bin/Release/netcoreapp2.1',
          targetDirectory: '',
          filePath: ''
        }
      }
    }
    stage('Deploy UI') {
      when { branch 'master' }
      steps {
        dir("ui/dist/journally") {
          azureUpload blobProperties: [
            cacheControl: '',
            contentEncoding: '',
            contentLanguage: '',
            contentType: '',
            detectContentType: true],
            containerName: '$web',
            fileShareName: '',
            filesPath: '**/*',
            storageCredentialId: 'journally',
            storageType: 'blobstorage'
        }
      }
    }
    stage('Purge CDN') {
      when { branch 'master' }
      steps {
        sh 'az cdn endpoint purge -g journally-prod-rg  --profile-name journally-prod-cdn --name journally-prod-cdnEndpoint --content-paths /'
      }
    }
  }
  post {
    failure {
      slackSend color: 'danger', message: "Journally deployment failed (<${env.BUILD_URL}|Open>)"
    }
    /*always {
      cleanWs()
    }*/
  }
}
