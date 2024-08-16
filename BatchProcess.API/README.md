# Publications.Authors

## Docker
Notes: From the Publications folder run...

1) Build the image
```bash
docker build -f Dockerfile.Publications.Authors --tag publications.authors .
```

2) Create and run the container
```bash
docker run --name publications.authors -p 7000:80 -p 7001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=7001 publications.authors
```

## Nuget

### Clear Cash
```bash
nuget locals all -clear
```
### Create nuget package
Notes: from project directory
```bash
dotnet pack -o ..\packages\
```

## Simple Certificate
```bash
dotnet dev-certs https --trust
```

## Certificate
```bash
$cert = New-SelfSignedCertificate -certstorelocation cert:\localmachine\my -dns publications.io
$pwd = ConvertTo-SecureString -String "Ci281978!" -Force -AsPlainText
$certpath = "Cert:\localmachine\my\$($cert.Thumbprint)"
Export-PfxCertificate -Cert $certpath -FilePath C:\Users\ciordanidis\.aspnet\https\publications.io.pfx -Password $pwd
dotnet user-secrets set "CertPassword" "Ci281978!"
dotnet dev-certs https --trust
```

## Seq logging database
  1. Username admin
  2. Password root

## Swagger
  Path: {Domain}/swagger/index.html