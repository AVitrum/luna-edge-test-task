#!/bin/bash
set -e

chown -R "${APP_UID}":"${APP_UID}" /https

CERT_PATH="/https/myproject.pfx"

gosu "${APP_UID}" bash <<EOF
if [ ! -f "$CERT_PATH" ]; then
    echo "Development certificate not found. Generating a new one..."
    dotnet dev-certs https -ep "$CERT_PATH" -p "\$ASPNETCORE_Kestrel__Certificates__Default__Password"
else
    echo "Development certificate found."
fi
EOF

exec gosu "${APP_UID}" dotnet MyProject.WebApi.dll