#!/bin/bash

dotnet lambda deploy-serverless \
  --stack-name Dnw-Website \
  --s3-bucket dnw-templates-2022

aws cloudfront create-invalidation \
  --distribution-id E2LR1K2KPCV2U8 \
  --paths "/*"