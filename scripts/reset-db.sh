#!/usr/bin/env sh
set -eu
name="${POSTGRES_DB:-ecma_dev}"
case "$name" in *dev*|*DEV*|*test*|*TEST*) ;; *) echo "Refusing to reset database '$name'; its name must contain dev or test." >&2; exit 1;; esac
export ConnectionStrings__ECMA="Host=${POSTGRES_HOST:-localhost};Port=${POSTGRES_PORT:-5432};Database=$name;Username=${POSTGRES_USER:?required};Password=${POSTGRES_PASSWORD:?required}"
dotnet run --project src/ECMA.Web/ECMA.Web.csproj -- --reset-db
