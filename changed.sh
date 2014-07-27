#!/bin/bash

git log --pretty=format:%s `git describe --abbrev=0 --tags`..HEAD