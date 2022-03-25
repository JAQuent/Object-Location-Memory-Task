# Script to run generate trial data for AMT version 1 (standard)
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
setwd("~/Unity/Arena Memory Task/nonUnity_folder/trialData/")
######################################################

# Libs
library(plyr)


# /* 
# ----------------------------- Parameters --------------------------
# */
numPerBlock <- 12
blockNum    <- 7
targets     <- 1:6
seed        <- 20220324

# /* 
# ----------------------------- Generate --------------------------
# */
trialData <- data.frame(block_num = rep(1:blockNum, each = numPerBlock),
                        targets = rep(targets, each = 2))


# /* 
# ----------------------------- Shuffle --------------------------
# */
set.seed(seed)
trialData_shuffled <- ddply(trialData, ("block_num"), mutate, targets = sample(targets))
names(trialData_shuffled) <- c('block_num', 'targets')


# /* 
# ----------------------------- Write 2 file --------------------------
# */
write.table(trialData_shuffled, "trialData.csv", quote = FALSE, row.names = FALSE, sep = ",", col.names = TRUE)
