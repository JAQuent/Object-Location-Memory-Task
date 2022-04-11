# Script to run generate trial data for OLM task
# Version 1.0
# Date:  24/03/2022
# Author: Joern Alexander Quent
# /* 
# ----------------------------- Note on this script ---------------------------
# */
#
# /* 
# ----------------------------- Libraries, settings and functions ---------------------------
# */
######################################################
setwd("~/Unity/Arena-Memory-Task/nonUnity_folder/trialData")
######################################################

# Libs
library(plyr)


# /* 
# ----------------------------- Parameters --------------------------
# */
#  Init
seed        <- 20220324

# Trials and blocks
numPerBlock <- 12
blockNum    <- 7
targets     <- 1:6


# Duration variables
cue         <- 2 # in s
delays      <- c(2, 3, 3.5, 4.5, 5, 6) # in s
ITIs        <- c(0.5, 1, 1.5, 2) # in s

# Speeds
speedForward <- 8.0 # in vm/s
rotationSpeed <- 30.0

# /* 
# ----------------------------- Generate --------------------------
# */
trialData <- data.frame(block_num = rep(1:blockNum, each = numPerBlock),
                        targets = rep(targets, each = 2),
                        speedForward = speedForward,
                        rotationSpeed = rotationSpeed,
                        cue = cue,
                        delay = rep(delays, (numPerBlock*blockNum)/length(delays)),
                        ITI  = rep(ITIs, (numPerBlock*blockNum)/length(ITIs)),
                        trialType = 'standard')


# /* 
# ----------------------------- Shuffle --------------------------
# */
set.seed(seed)
trialData_shuffled <- ddply(trialData, ("block_num"), mutate, targets = sample(targets), delay = sample(delay), ITI = sample(ITI))


# /* 
# ----------------------------- Add control trials --------------------------
# */
controlTrials <- data.frame(block_num = 8,
                            targets = 7,
                            speedForward = speedForward,
                            rotationSpeed = rotationSpeed,
                            cue = 1,
                            delay = 2,
                            ITI  = 1.5,
                            trialType = 'control')

trialData_shuffled <- rbind(trialData_shuffled, controlTrials)


# /* 
# ----------------------------- Write 2 file --------------------------
# */
write.table(trialData_shuffled, "trialData.csv", quote = FALSE, row.names = FALSE, sep = ",", col.names = TRUE)
