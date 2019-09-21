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
        dir("api") {
          azureFunctionAppPublish appName: "blujournal",
          azureCredentialsId: 'jenkins-sp',
          resourceGroup: "bluJournal-prod-rg",
          sourceDirectory: 'bin/Release/netcoreapp2.1',
          targetDirectory: '',
          filePath: ''
        }
      }
    }
    stage('Deploy UI') {
      when { branch 'master' }
      steps {
        dir("ui/dist/blu-journal") {
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
    stage('Purge CDN') {
      when { branch 'master' }
      steps {
        az cdn endpoint purge -g bluJournal-prod-rg  --profile-name bluJournal-prod-cdn --name bluJournal-prod-cdnEndpoint --content-paths /
        slackSend color: 'good', message: "bluJournal deployment successful => https://blujournal.com"
      }
    }
  }
  post {
    failure {
      slackSend color: 'danger', message: "bluJournal deployment failed (<${env.BUILD_URL}|Open>)"
    }
    always {
      cleanWs()
    }
  }
}
