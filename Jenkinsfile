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
          sh 'npm i'
          sh 'npm run build -- --prod'
        }
      }
    }
    stage('Deploy API') {
      when { branch 'master' }
      steps {
        azureFunctionAppPublish appName: "blujournal",
          azureCredentialsId: 'jenkins-sp',
          resourceGroup: "bluJournal-prod-rg",
          sourceDirectory: 'bin/Release/netcoreapp2.1',
          targetDirectory: '',
          filePath: ''
      }
    }
    stage('Deploy UI') {
      when { branch 'master' }
      steps {
        dir("cli/dist") {
          azureUpload blobProperties: [
            cacheControl: '',
            contentEncoding: '',
            contentLanguage: '',
            contentType: '',
            detectContentType: true],
            containerName: '$web',
            fileShareName: '',
            filesPath: '**/*',
            storageCredentialId: 'bluJournal',
            storageType: 'blobstorage'
        }
      }
    }
  }
  post {
    always {
      cleanWs()
    }
  }
}
